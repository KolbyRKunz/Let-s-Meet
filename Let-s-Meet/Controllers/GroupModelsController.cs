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
    public class GroupModelsController : Controller
    {
        private readonly MeetContext _context;

        public GroupModelsController(MeetContext context)
        {
            _context = context;
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
    }
}
