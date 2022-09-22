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
    public class AttendanceModelsController : Controller
    {
        private readonly MeetContext _context;

        public AttendanceModelsController(MeetContext context)
        {
            _context = context;
        }

        // GET: AttendanceModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Attendance.ToListAsync());
        }

        // GET: AttendanceModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceModel = await _context.Attendance
                .FirstOrDefaultAsync(m => m.AttendanceID == id);
            if (attendanceModel == null)
            {
                return NotFound();
            }

            return View(attendanceModel);
        }

        // GET: AttendanceModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AttendanceModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttendanceID,UserID,EventID")] AttendanceModel attendanceModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendanceModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attendanceModel);
        }

        // GET: AttendanceModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceModel = await _context.Attendance.FindAsync(id);
            if (attendanceModel == null)
            {
                return NotFound();
            }
            return View(attendanceModel);
        }

        // POST: AttendanceModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttendanceID,UserID,EventID")] AttendanceModel attendanceModel)
        {
            if (id != attendanceModel.AttendanceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendanceModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceModelExists(attendanceModel.AttendanceID))
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
            return View(attendanceModel);
        }

        // GET: AttendanceModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceModel = await _context.Attendance
                .FirstOrDefaultAsync(m => m.AttendanceID == id);
            if (attendanceModel == null)
            {
                return NotFound();
            }

            return View(attendanceModel);
        }

        // POST: AttendanceModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendanceModel = await _context.Attendance.FindAsync(id);
            _context.Attendance.Remove(attendanceModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceModelExists(int id)
        {
            return _context.Attendance.Any(e => e.AttendanceID == id);
        }
    }
}
