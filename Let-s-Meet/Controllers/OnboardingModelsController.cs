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
    public class OnboardingModelsController : Controller
    {
        private readonly MeetContext _context;

        public OnboardingModelsController(MeetContext context)
        {
            _context = context;
        }

        // GET: OnboardingModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Onboarding.ToListAsync());
        }

        // GET: OnboardingModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onboardingModel = await _context.Onboarding
                .FirstOrDefaultAsync(m => m.OnboardingID == id);
            if (onboardingModel == null)
            {
                return NotFound();
            }

            return View(onboardingModel);
        }

        // GET: OnboardingModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OnboardingModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OnboardingID,UserID,LastStepCompleted")] OnboardingModel onboardingModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(onboardingModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(onboardingModel);
        }

        // GET: OnboardingModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onboardingModel = await _context.Onboarding.FindAsync(id);
            if (onboardingModel == null)
            {
                return NotFound();
            }
            return View(onboardingModel);
        }

        // POST: OnboardingModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OnboardingID,UserID,LastStepCompleted")] OnboardingModel onboardingModel)
        {
            if (id != onboardingModel.OnboardingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(onboardingModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OnboardingModelExists(onboardingModel.OnboardingID))
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
            return View(onboardingModel);
        }

        // GET: OnboardingModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onboardingModel = await _context.Onboarding
                .FirstOrDefaultAsync(m => m.OnboardingID == id);
            if (onboardingModel == null)
            {
                return NotFound();
            }

            return View(onboardingModel);
        }

        // POST: OnboardingModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var onboardingModel = await _context.Onboarding.FindAsync(id);
            _context.Onboarding.Remove(onboardingModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OnboardingModelExists(int id)
        {
            return _context.Onboarding.Any(e => e.OnboardingID == id);
        }
    }
}
