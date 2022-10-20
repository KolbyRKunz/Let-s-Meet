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
using Let_s_Meet.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;
using Let_s_Meet.Migrations;

namespace Let_s_Meet.Controllers
{
    [Authorize]
    public class FriendsModelsController : Controller
    {
        private readonly MeetContext _context;
        private readonly UserManager<User> _um;

        public FriendsModelsController(MeetContext context, UserManager<User> userManager)
        {
            _context = context;
            _um = userManager;
        }

        // GET: FriendsModels
        public async Task<IActionResult> Index()
        {
            var meetContext = _context.Friends.Include(f => f.RequestedBy).Include(f => f.RequestedTo);
            return View(await meetContext.ToListAsync());
        }

        // GET: FriendsModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendsModel = await _context.Friends
                .Include(f => f.RequestedBy)
                .Include(f => f.RequestedTo)
                .FirstOrDefaultAsync(m => m.FriendsID == id);
            if (friendsModel == null)
            {
                return NotFound();
            }

            return View(friendsModel);
        }

        // GET: FriendsModels/Create
        public IActionResult Create()
        {
            ViewData["RequestedByID"] = new SelectList(_context.Users, "UserID", "UserID");
            ViewData["RequestedToID"] = new SelectList(_context.Users, "UserID", "UserID");
            return View();
        }

        // POST: FriendsModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FriendsID,RequestedByID,RequestedToID,RequestStatus")] FriendsModel friendsModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(friendsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RequestedByID"] = new SelectList(_context.Users, "UserID", "UserID", friendsModel.RequestedByID);
            ViewData["RequestedToID"] = new SelectList(_context.Users, "UserID", "UserID", friendsModel.RequestedToID);
            return View(friendsModel);
        }

        // GET: FriendsModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendsModel = await _context.Friends.FindAsync(id);
            if (friendsModel == null)
            {
                return NotFound();
            }
            ViewData["RequestedByID"] = new SelectList(_context.Users, "UserID", "UserID", friendsModel.RequestedByID);
            ViewData["RequestedToID"] = new SelectList(_context.Users, "UserID", "UserID", friendsModel.RequestedToID);
            return View(friendsModel);
        }

        // POST: FriendsModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FriendsID,RequestedByID,RequestedToID,RequestStatus")] FriendsModel friendsModel)
        {
            if (id != friendsModel.FriendsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(friendsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FriendsModelExists(friendsModel.FriendsID))
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
            ViewData["RequestedByID"] = new SelectList(_context.Users, "UserID", "UserID", friendsModel.RequestedByID);
            ViewData["RequestedToID"] = new SelectList(_context.Users, "UserID", "UserID", friendsModel.RequestedToID);
            return View(friendsModel);
        }

        // GET: FriendsModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendsModel = await _context.Friends
                .Include(f => f.RequestedBy)
                .Include(f => f.RequestedTo)
                .FirstOrDefaultAsync(m => m.FriendsID == id);
            if (friendsModel == null)
            {
                return NotFound();
            }

            return View(friendsModel);
        }

        // POST: FriendsModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var friendsModel = await _context.Friends.FindAsync(id);
            _context.Friends.Remove(friendsModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FriendsModelExists(int id)
        {
            return _context.Friends.Any(e => e.FriendsID == id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFriendRequestById(int friendId)
        {
            // Check if user exists with friend ID
            UserModel friend = await _context.Users.FindAsync(friendId);

            if (friend == null)
                return Ok(new { status = "error", messsage = "User does not exist" });

            return await CreateFriendRequest(friend);
        }

        // TODO get UserModels to have emails
        //[HttpPost]
        //public async Task<IActionResult> CreateFriendRequestByEmail(string email)
        //{
        //    // Check if user exists with friend email
        //    UserModel friend = await _context.Users.Where(u => u.email)
        //}

        private async Task<IActionResult> CreateFriendRequest(UserModel friend)
        {
            // Get current user's ID
            User user = await _um.GetUserAsync(User);
            int userId = user.UserID;

            // Check if friend request already exists
            var friendsModel = await _context
                .Friends
                .Where(r => 
                    (r.RequestedByID == userId && r.RequestedToID == friend.UserID)
                    ||
                    (r.RequestedByID == friend.UserID && r.RequestedToID == userId)
                )
                .FirstOrDefaultAsync();

            // If the request exists
            if (friendsModel != null)
            {
                switch (friendsModel.RequestStatus)
                {
                    // If the request is already accepted, return that they are already friends
                    case FriendRequestStatus.Accepted:
                        return Ok(new { status = "error", message = "Already friends with user." }); 
                        
                    // If the request is pending, return that they are there is a pending request
                    case FriendRequestStatus.Sent:
                        // If from current user
                        if (friendsModel.RequestedByID == userId)
                            return Ok(new { status = "error", message = "Friend request already sent, pending." });

                        // If from friend
                        else
                            return Ok(new { status = "error", message = "Friend request already received, pending." });
                        
                    // If the request is rejected, resend i.e. set to Sent
                    default: // Default equivalent to Rejected b/c it is the only other enum value
                        friendsModel.RequestStatus = FriendRequestStatus.Rejected;
                        _context.Update(friendsModel);
                        await _context.SaveChangesAsync();
                        return Ok(new { status = "ok", message = "Friend request sent." });
                }
            }

            // Create request
            else
            {
                // Get current user's model
                UserModel userModel = await _context.Users.FindAsync(userId);

                friendsModel = new FriendsModel
                {
                    RequestedBy = userModel,
                    RequestedByID = userModel.UserID,
                    RequestedTo = friend,
                    RequestedToID = friend.UserID,
                    RequestStatus = FriendRequestStatus.Sent
                };

                _context.Add(friendsModel);
                await _context.SaveChangesAsync();

                // TODO notify friend

                return Ok(new {status = "ok", message = "Friend request sent."});
            }
        }
    }
}
