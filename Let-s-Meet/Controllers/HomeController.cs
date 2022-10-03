using Let_s_Meet.Areas.Identity.Data;
using Let_s_Meet.Data;
using Let_s_Meet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Let_s_Meet.Controllers
{
     //This is to make it so users have to be logged in before accessing the site
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _um;
        private readonly MeetContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<User> um, MeetContext context)
        {
            _logger = logger;
            _um = um;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.Name == null)
            {
                return new RedirectResult("/Auth");
            }
            // Put list of user's calendars as JSON in ViewBag
            User user = _um.GetUserAsync(User).Result;
            UserModel userModel = _context.Users.Find(user.UserID);
            List<CalendarModel> cals = _context.Calendars.Where(c => c.Owner == userModel).ToList();

            // TODO fix infinite loop properly
            // Remove Owners from cals
            foreach (CalendarModel cal in cals)
            {
                cal.Owner = null;
            }

            ViewBag.Calendars = JsonConvert.SerializeObject(cals);

            return View();
        }
        
        public IActionResult Groups()
        {
            if (User.Identity.Name == null)
            {
                return new RedirectResult("/Auth");
            }

            return View();
        }

        public IActionResult Friends()
        {
            if (User.Identity.Name == null)
            {
                return new RedirectResult("/Auth");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
