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
    public class SettingsModelsController : Controller
    {
        private readonly MeetContext _context;

        public SettingsModelsController(MeetContext context)
        {
            _context = context;
        }

        // GET: SettingsModels
        public async Task<IActionResult> Index()
        {
            var meetContext = _context.Settings.Include(s => s.User);
            return View(await meetContext.ToListAsync());
        }

        // GET: SettingsModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settingsModel = await _context.Settings
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.SettingsID == id);
            if (settingsModel == null)
            {
                return NotFound();
            }

            return View(settingsModel);
        }

        // GET: SettingsModels/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID");
            return View();
        }

        // POST: SettingsModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SettingsID,UserID")] SettingsModel settingsModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(settingsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", settingsModel.UserID);
            return View(settingsModel);
        }

        // GET: SettingsModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settingsModel = await _context.Settings.FindAsync(id);
            if (settingsModel == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", settingsModel.UserID);
            return View(settingsModel);
        }

        // POST: SettingsModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SettingsID,UserID")] SettingsModel settingsModel)
        {
            if (id != settingsModel.SettingsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(settingsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettingsModelExists(settingsModel.SettingsID))
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
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", settingsModel.UserID);
            return View(settingsModel);
        }

        // GET: SettingsModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settingsModel = await _context.Settings
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.SettingsID == id);
            if (settingsModel == null)
            {
                return NotFound();
            }

            return View(settingsModel);
        }

        // POST: SettingsModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var settingsModel = await _context.Settings.FindAsync(id);
            _context.Settings.Remove(settingsModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SettingsModelExists(int id)
        {
            return _context.Settings.Any(e => e.SettingsID == id);
        }
    }
}
