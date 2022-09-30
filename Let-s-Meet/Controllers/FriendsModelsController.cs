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

namespace Let_s_Meet.Controllers
{
    [Authorize]
    public class FriendsModelsController : Controller
    {
        private readonly MeetContext _context;

        public FriendsModelsController(MeetContext context)
        {
            _context = context;
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
    }
}
