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
    public class CalendarModelsController : Controller
    {
        private readonly MeetContext _context;

        public CalendarModelsController(MeetContext context)
        {
            _context = context;
        }

        // GET: CalendarModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Calendar.ToListAsync());
        }

        // GET: CalendarModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarModel = await _context.Calendar
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
        public async Task<IActionResult> Create([Bind("CalendarID,Name,Description,Color")] CalendarModel calendarModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(calendarModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(calendarModel);
        }

        // GET: CalendarModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calendarModel = await _context.Calendar.FindAsync(id);
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

            var calendarModel = await _context.Calendar
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
            var calendarModel = await _context.Calendar.FindAsync(id);
            _context.Calendar.Remove(calendarModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CalendarModelExists(int id)
        {
            return _context.Calendar.Any(e => e.CalendarID == id);
        }
    }
}
