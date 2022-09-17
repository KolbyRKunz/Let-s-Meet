using Let_s_Meet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Let_s_Meet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Identity.Name == null)
            {
                return new RedirectResult("/Auth");
            }
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
