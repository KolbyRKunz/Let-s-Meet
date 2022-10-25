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

        [HttpPost]
        public async Task<IActionResult> CreateGroup(string name, List<int> friendIds)
        {
            User user = await _um.GetUserAsync(User);
            int userId = user.UserID;

            // Ensure ids are friends
            var friends = await _context
                .Friends
                .Where(f =>
                    (
                        (f.RequestedByID == userId && friendIds.Contains(f.RequestedToID))
                        ||
                        (f.RequestedToID == userId && friendIds.Contains(f.RequestedByID))
                    )
                    &&
                    f.RequestStatus == FriendRequestStatus.Accepted
                )
                .ToListAsync();

            if (friends.Count != new HashSet<int>(friendIds).Count)
            {
                return BadRequest("Not all ids are friends");
            }

            // Get list of UserModels for friends
            List<UserModel> users = await _context
                .Users
                .Where(u => friendIds.Contains(u.UserID))
                .ToListAsync();
            UserModel userModel = await _context.Users.FindAsync(userId);
            users.Add(userModel);

            // Create group
            GroupModel group = new GroupModel
            {
                GroupName = name,
                Users = users
            };
            _context.Add(group);
            await _context.SaveChangesAsync();

            return Ok(new { status = "ok", message = "Group created" });
        }
    }
}
