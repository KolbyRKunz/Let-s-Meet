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

namespace Let_s_Meet.Controllers
{
    [Authorize]
    public class CalendarModelsController : Controller
    {
        private readonly MeetContext _context;
        private readonly UserManager<User> _um;

        public CalendarModelsController(MeetContext context, UserManager<User> userManager)
        {
            _context = context;
            _um = userManager;
        }

        // GET: CalendarModels
        public async Task<IActionResult> Index()
        {
            if (User.Identity.Name == null)
            {
                return new RedirectResult("/Auth");
            }

            User user = await _um.GetUserAsync(User);
            int id = user.UserID;
            //return View(await _context.Calendar.ToListAsync()); // This is for getting all calendars

            // Get current user's calendars
            return View(await _context
                .Calendars
                .Include(c => c.Owner)
                .Include(c => c.Group)
                .Where(c => c.Owner.UserID == id || c.Group.Users.Any(m => m.UserID == id))
                .ToListAsync()
                );
        }

        // GET: CalendarModels/GetCalendars
        public async Task<IActionResult> GetCalendars()
        {
            User user = await _um.GetUserAsync(User);
            int id = user.UserID;
            //return Ok(await _context.Calendar.ToListAsync()); // This is for getting all calendars

            // Get user model
            UserModel userModel = await _context.Users.Where(u => u.UserID == id).FirstOrDefaultAsync();

            // If no user model
            if (userModel == null)
            {
                return NotFound();
            }

            // Get current user's calendars
            return Ok(await _context
                .Calendars
                .Include(c => c.Owner)
                .Where(c => c.Owner.UserID == id || c.Group.Users.Any(m => m.UserID == id))
                .Select(c => new
                    {
                        c.CalendarID,
                        c.Name,
                        c.Description,
                        c.Color,
                        c.Group,
                        c.Group.Users
                    })
                .ToListAsync()
                );
        }

        // GET: CalendarModels/GetCalendar
        public async Task<IActionResult> GetCalendar(int id)
        {
            User user = await _um.GetUserAsync(User);
            int userId = user.UserID;
            //return Ok(await _context.Calendar.ToListAsync()); // This is for getting all calendars

            // Get user model
            UserModel userModel = await _context.Users.Where(u => u.UserID == userId).FirstOrDefaultAsync();

            // If no user model
            if (userModel == null)
            {
                return NotFound();
            }

            // Get current user's calendars
            return Ok(await _context
                .Calendars
                .Include(c => c.Owner)
                .Where(c => c.CalendarID == userId && (c.Owner.UserID == userId || c.Group.Users.Any(m => m.UserID == userId)))
                .Select(c => new
                {
                    c.CalendarID,
                    c.Name,
                    c.Description,
                    c.Color,
                    c.Group,
                    c.Group.Users
                })
                .FirstOrDefaultAsync()
                );
        }

        // GET: CalendarModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarModel = await _context.Calendars
                .Include(c => c.Group)
                .FirstOrDefaultAsync(m => m.CalendarID == id);
            if (calendarModel == null)
            {
                return NotFound();
            }

            return View(calendarModel);
        }

        // GET: CalendarModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CalendarModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CalendarID,Name,Description,Color,Owner")] CalendarModel calendarModel)
        {
            // get current user and set it as owner
            User user = await _um.GetUserAsync(User);
            UserModel userModel = _context.Users.Find(user.UserID);
            calendarModel.Owner = userModel;

            if (ModelState.IsValid)
            {
                _context.Add(calendarModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(calendarModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCalendar([Bind("CalendarID,Name,Description,Color,Owner")] CalendarModel calendarModel)
        {
            // get current user and set it as owner
            User user = await _um.GetUserAsync(User);
            UserModel userModel = _context.Users.Find(user.UserID);
            calendarModel.Owner = userModel;

            if (ModelState.IsValid)
            {
                _context.Add(calendarModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return Ok(calendarModel);
        }


        // GET: CalendarModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarModel = await _context.Calendars
                .Include(c => c.Group)
                .FirstOrDefaultAsync(c => c.CalendarID == id);
            if (calendarModel == null)
            {
                return NotFound();
            }
            return View(calendarModel);
        }

        // POST: CalendarModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CalendarID,Name,Description,Color")] CalendarModel calendarModel)
        {
            if (id != calendarModel.CalendarID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calendarModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalendarModelExists(calendarModel.CalendarID))
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
            return View(calendarModel);
        }

        // GET: CalendarModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarModel = await _context.Calendars
                .FirstOrDefaultAsync(m => m.CalendarID == id);
            if (calendarModel == null)
            {
                return NotFound();
            }

            return View(calendarModel);
        }

        // POST: CalendarModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var calendarModel = await _context.Calendars.FindAsync(id);
            _context.Calendars.Remove(calendarModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CalendarModelExists(int id)
        {
            return _context.Calendars.Any(e => e.CalendarID == id);
        }
    }
}
