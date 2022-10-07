﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Let_s_Meet.Data;
using Let_s_Meet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Let_s_Meet.Areas.Identity.Data;
using System.Globalization;

namespace Let_s_Meet.Controllers
{
    [Authorize]
    public class EventModelsController : Controller
    {
        private readonly MeetContext _context;
        private readonly UserManager<User> _um;

        public EventModelsController(MeetContext context, UserManager<User> userManager)
        {
            _context = context;
            _um = userManager;
        }

        // GET: EventModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        // GET: EventModels/Mine
        public async Task<IActionResult> GetEvents()
        {
            User user = await _um.GetUserAsync(User);
            int id = user.UserID;
            var events = await _context
                .Events
                .Include(e => e.Users)
                .Include(e => e.Calendar)
                .Where(e => e.Users.Any(u => u.UserID == id))
                .Select(e => new
                {
                    id = e.EventID,
                    title = e.Title,
                    start = e.StartTime.ToString("O"),
                    end = e.EndTime.ToString("O"),
                    location = e.Location,
                    color = e.Calendar.Color,
                    background = e.Calendar.Color,
                    backgroundColor = e.Calendar.Color,
                    // TODO users w/o infinite loop
                })
                .ToListAsync();
            return Ok(events);
        }
        

        // GET: EventModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventModel = await _context.Events
                .FirstOrDefaultAsync(m => m.EventID == id);
            if (eventModel == null)
            {
                return NotFound();
            }

            return View(eventModel);
        }

        // GET: EventModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(string title, string location, string startTime, string endTime, int calendarID)
        {
            var styles = DateTimeStyles.AdjustToUniversal;
            var format = CultureInfo.InvariantCulture;
            EventModel eventModel = new EventModel {
                Title = title,
                Location = location,
                StartTime = DateTime.Parse(startTime,format, styles),
                EndTime = DateTime.Parse(endTime, format, styles)
            };

            User user = await _um.GetUserAsync(User);
            UserModel userModel = await _context.Users.FindAsync(user.UserID);
            CalendarModel cal = await _context.Calendars.FindAsync(calendarID);

            List<UserModel> users = new List<UserModel> { userModel };
            
            eventModel.Users = users;
            eventModel.Calendar = cal;

            // Add to database
            _context.Events.Add(eventModel);
            await _context.SaveChangesAsync();

            return Ok(new {
                StartTime = eventModel.StartTime,
                EndTime = eventModel.EndTime,
                Title = eventModel.Title,
                Location = eventModel.Location,
                //Users = users // TODO format for no infinite
            });
        }

        // GET: EventModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventModel = await _context.Events.FindAsync(id);
            if (eventModel == null)
            {
                return NotFound();
            }
            return View(eventModel);
        }

        // POST: EventModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,startTime,endTime")] EventModel eventModel)
        {
            if (id != eventModel.EventID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventModelExists(eventModel.EventID))
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
            return View(eventModel);
        }

        // GET: EventModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventModel = await _context.Events
                .FirstOrDefaultAsync(m => m.EventID == id);
            if (eventModel == null)
            {
                return NotFound();
            }

            return View(eventModel);
        }

        // POST: EventModels/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventModel = await _context.Events.FindAsync(id);
            _context.Events.Remove(eventModel);
            await _context.SaveChangesAsync();
            return Ok();//RedirectToAction(nameof(Index));
        }

        private bool EventModelExists(int id)
        {
            return _context.Events.Any(e => e.EventID == id);
        }
    }
}
