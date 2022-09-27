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
    public class EventPrivacyModelsController : Controller
    {
        private readonly MeetContext _context;

        public EventPrivacyModelsController(MeetContext context)
        {
            _context = context;
        }

        // GET: EventPrivacyModels
        public async Task<IActionResult> Index()
        {
            var meetContext = _context.EventPrivacy.Include(e => e.Event);
            return View(await meetContext.ToListAsync());
        }

        // GET: EventPrivacyModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventPrivacyModel = await _context.EventPrivacy
                .Include(e => e.Event)
                .FirstOrDefaultAsync(m => m.EventPrivacyID == id);
            if (eventPrivacyModel == null)
            {
                return NotFound();
            }

            return View(eventPrivacyModel);
        }

        // GET: EventPrivacyModels/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventID", "EventID");
            return View();
        }

        // POST: EventPrivacyModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventPrivacyID,EventId")] EventPrivacyModel eventPrivacyModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventPrivacyModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventID", "EventID", eventPrivacyModel.EventId);
            return View(eventPrivacyModel);
        }

        // GET: EventPrivacyModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventPrivacyModel = await _context.EventPrivacy.FindAsync(id);
            if (eventPrivacyModel == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventID", "EventID", eventPrivacyModel.EventId);
            return View(eventPrivacyModel);
        }

        // POST: EventPrivacyModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventPrivacyID,EventId")] EventPrivacyModel eventPrivacyModel)
        {
            if (id != eventPrivacyModel.EventPrivacyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventPrivacyModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventPrivacyModelExists(eventPrivacyModel.EventPrivacyID))
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
            ViewData["EventId"] = new SelectList(_context.Events, "EventID", "EventID", eventPrivacyModel.EventId);
            return View(eventPrivacyModel);
        }

        // GET: EventPrivacyModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventPrivacyModel = await _context.EventPrivacy
                .Include(e => e.Event)
                .FirstOrDefaultAsync(m => m.EventPrivacyID == id);
            if (eventPrivacyModel == null)
            {
                return NotFound();
            }

            return View(eventPrivacyModel);
        }

        // POST: EventPrivacyModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventPrivacyModel = await _context.EventPrivacy.FindAsync(id);
            _context.EventPrivacy.Remove(eventPrivacyModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventPrivacyModelExists(int id)
        {
            return _context.EventPrivacy.Any(e => e.EventPrivacyID == id);
        }
    }
}
