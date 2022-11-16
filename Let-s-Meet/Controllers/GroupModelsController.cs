using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Let_s_Meet.Data;
using Let_s_Meet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Let_s_Meet.Areas.Identity.Data;
using Let_s_Meet.Models.FromBodyDataModels;

namespace Let_s_Meet.Controllers
{
    [Authorize]
    public class GroupModelsController : Controller
    {
        private readonly MeetContext _context;
        private readonly UserManager<User> _um;

        public GroupModelsController(MeetContext context, UserManager<User> um)
        {
            _context = context;
            _um = um;
        }

        // GET: GroupModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Groups.ToListAsync());
        }

        // GET: GroupModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupModel = await _context.Groups
                .FirstOrDefaultAsync(m => m.GroupID == id);
            if (groupModel == null)
            {
                return NotFound();
            }

            return View(groupModel);
        }

        // GET: GroupModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GroupModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupID,GroupName")] GroupModel groupModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(groupModel);
        }

        // GET: GroupModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupModel = await _context.Groups.FindAsync(id);
            if (groupModel == null)
            {
                return NotFound();
            }
            return View(groupModel);
        }

        // POST: GroupModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupID,GroupName")] GroupModel groupModel)
        {
            if (id != groupModel.GroupID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupModelExists(groupModel.GroupID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(groupModel);
        }

        // GET: GroupModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupModel = await _context.Groups
                .FirstOrDefaultAsync(m => m.GroupID == id);
            if (groupModel == null)
            {
                return NotFound();
            }

            return View(groupModel);
        }

        // POST: GroupModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupModel = await _context.Groups.FindAsync(id);
            _context.Groups.Remove(groupModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupModelExists(int id)
        {
            return _context.Groups.Any(e => e.GroupID == id);
        }

        public async Task<IActionResult> GetGroups()
        {
            User user = await _um.GetUserAsync(User);
            int userId = user.UserID;

            // Get groups that user is a member of
            var groups = await _context.Groups
                .Where(g => g.Users.Any(m => m.UserID == userId))
                .ToListAsync();

            // TODO look for issues with users of groups (JSON cycle?)
            return Ok(groups);
        }

        public async Task<IActionResult> GetGroup(int id)
        {
            User user = await _um.GetUserAsync(User);
            int userId = user.UserID;

            // Get group
            var group = await _context.Groups
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.GroupID == id && g.Users.Any(m => m.UserID == userId));

            // If no group or user is not a member of group
            if (group == null) return Ok(null);

            // Format user objects
            var users = group.Users.Select(u => new UserModel{ 
                UserID = u.UserID, 
                FirstName = u.FirstName, 
                LastName = u.LastName, 
                Email = u.Email 
            });
            group.Users = users.ToList();

            return Ok(group);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] GroupCreationModel groupInfo)
        {
            User user = await _um.GetUserAsync(User);
            int userId = user.UserID;

            // Ensure ids are friends
            var friends = await _context
                .Friends
                .Where(f =>
                    (
                        (f.RequestedByID == userId && groupInfo.friendIds.Contains(f.RequestedToID))
                        ||
                        (f.RequestedToID == userId && groupInfo.friendIds.Contains(f.RequestedByID))
                    )
                    &&
                    f.RequestStatus == FriendRequestStatus.Accepted
                )
                .ToListAsync();

            if (friends.Count != new HashSet<int>(groupInfo.friendIds).Count)
            {
                return BadRequest("Not all ids are friends");
            }

            // Get list of UserModels for friends
            List<UserModel> users = await _context
                .Users
                .Where(u => groupInfo.friendIds.Contains(u.UserID))
                .ToListAsync();
            UserModel userModel = await _context.Users.FindAsync(userId);
            users.Add(userModel);
            
            
            // Create group
            GroupModel group = new GroupModel
            {
                GroupName = groupInfo.name,
                Users = users
            };
            _context.Add(group);
            await _context.SaveChangesAsync();

            // Generate random join code with format <group id>-<random alphanumeric string with length 12>
            string joinCode = group.GroupID + "-" + Guid.NewGuid().ToString().Substring(0, 8);

            // Update group with join code
            group.JoinCode = joinCode;
            _context.Update(group);
            await _context.SaveChangesAsync();

            /*IMPORTANT: change this if needed
                this should assign each group a calendar upon creation*/
            CalendarModel cal = new CalendarModel
            {
                Name = group.GroupName + " Group Calendar",
                Description = group.GroupName + " Group Calendar",
                Color = "#950741",
                Group = group,
            };

            _context.Add(cal);
            await _context.SaveChangesAsync();

            group.CalendarID = cal.CalendarID;
            _context.Update(group);
            await _context.SaveChangesAsync();

            return Ok(new { status = "ok", message = "Group created" });
        }
        public async Task<IActionResult> JoinGroupRedirect(string joinCode)
        {
            var result = await JoinGroup(joinCode);
            if (result is OkObjectResult ok)
            {
                return RedirectToAction("Groups", "Home");
            }
            else
            {
                return result;
            }
        }

        [HttpPost]
        public async Task<IActionResult> JoinGroup(string joinCode)
        {
            User user = await _um.GetUserAsync(User);
            int userId = user.UserID;

            // Get group from join code
            string[] split = joinCode.Split('-');
            if (split.Length != 2)
            {
                return BadRequest("Invalid join code");
            }

            int groupId = int.Parse(split[0]);
            GroupModel group = await _context
                .Groups
                .Include(g => g.Users)
                .Where(g => g.GroupID == groupId && g.JoinCode == joinCode)
                .FirstOrDefaultAsync();
            if (group == null)
            {
                return BadRequest("Invalid join code");
            }

            // Ensure user is not already in group
            if (group.Users.Any(u => u.UserID == userId))
            {
                return Ok(new { status = "ok", message = "User already in group" });
            }

            // Add user to group
            UserModel userModel = await _context.Users.FindAsync(userId);
            group.Users.Add(userModel);
            _context.Update(group);
            await _context.SaveChangesAsync();

            return Ok(new { status = "ok", message = "User joined group" });
        }

        [HttpDelete]
        public async Task<IActionResult> LeaveGroup(int id)
        {
            User user = await _um.GetUserAsync(User);
            int userId = user.UserID;

            // Get user model
            UserModel userModel = await _context.Users.FindAsync(userId);

            // Get group with id
            GroupModel group = await _context
                .Groups
                .Include("Users")
                .Where(g => g.GroupID == id)
                .FirstOrDefaultAsync();

            if (group == null) return NotFound();

            group.Users.Remove(userModel);
            _context.Update(group);

            // TODO delete group if empty?

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
