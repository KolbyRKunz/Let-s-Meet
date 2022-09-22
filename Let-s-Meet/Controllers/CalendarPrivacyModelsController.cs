using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Let_s_Meet.Data;
using Let_s_Meet.Models;

namespace Let_s_Meet.Controllers
{
    public class CalendarPrivacyModelsController : Controller
    {
        private readonly MeetContext _context;

        public CalendarPrivacyModelsController(MeetContext context)
        {
            _context = context;
        }

        // GET: CalendarPrivacyModels
        public async Task<IActionResult> Index()
        {
            var meetContext = _context.CalendarPrivacy.Include(c => c.Calendar);
            return View(await meetContext.ToListAsync());
        }

        // GET: CalendarPrivacyModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarPrivacyModel = await _context.CalendarPrivacy
                .Include(c => c.Calendar)
                .FirstOrDefaultAsync(m => m.CalendarPrivacyID == id);
            if (calendarPrivacyModel == null)
            {
                return NotFound();
            }

            return View(calendarPrivacyModel);
        }

        // GET: CalendarPrivacyModels/Create
        public IActionResult Create()
        {
            ViewData["CalendarID"] = new SelectList(_context.Calendar, "CalendarID", "CalendarID");
            return View();
        }

        // POST: CalendarPrivacyModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CalendarPrivacyID,CalendarID")] CalendarPrivacyModel calendarPrivacyModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(calendarPrivacyModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CalendarID"] = new SelectList(_context.Calendar, "CalendarID", "CalendarID", calendarPrivacyModel.CalendarID);
            return View(calendarPrivacyModel);
        }

        // GET: CalendarPrivacyModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarPrivacyModel = await _context.CalendarPrivacy.FindAsync(id);
            if (calendarPrivacyModel == null)
            {
                return NotFound();
            }
            ViewData["CalendarID"] = new SelectList(_context.Calendar, "CalendarID", "CalendarID", calendarPrivacyModel.CalendarID);
            return View(calendarPrivacyModel);
        }

        // POST: CalendarPrivacyModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CalendarPrivacyID,CalendarID")] CalendarPrivacyModel calendarPrivacyModel)
        {
            if (id != calendarPrivacyModel.CalendarPrivacyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calendarPrivacyModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalendarPrivacyModelExists(calendarPrivacyModel.CalendarPrivacyID))
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
            ViewData["CalendarID"] = new SelectList(_context.Calendar, "CalendarID", "CalendarID", calendarPrivacyModel.CalendarID);
            return View(calendarPrivacyModel);
        }

        // GET: CalendarPrivacyModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarPrivacyModel = await _context.CalendarPrivacy
                .Include(c => c.Calendar)
                .FirstOrDefaultAsync(m => m.CalendarPrivacyID == id);
            if (calendarPrivacyModel == null)
            {
                return NotFound();
            }

            return View(calendarPrivacyModel);
        }

        // POST: CalendarPrivacyModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var calendarPrivacyModel = await _context.CalendarPrivacy.FindAsync(id);
            _context.CalendarPrivacy.Remove(calendarPrivacyModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CalendarPrivacyModelExists(int id)
        {
            return _context.CalendarPrivacy.Any(e => e.CalendarPrivacyID == id);
        }
    }
}
