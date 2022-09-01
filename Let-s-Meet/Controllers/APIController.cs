using Let_s_Meet.Data;
using Let_s_Meet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Let_s_Meet.Controllers
{
    public class APIController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MeetContext _context;

        //public OpportunitiesController(URC_Context context)
        //{
        //    _context = context;
        //}

        // GET: Opportunities
        //[AllowAnonymous]
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Opportunities.ToListAsync());
        //}

        public APIController(ILogger<HomeController> logger, MeetContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public OkObjectResult Post(string data)
        {
            return Ok(new { message = "Hello from web server!!" + data });
        }

        /// <summary>
        /// Get all events related to the currently signed in user
        /// </summary>
        /// <returns>json object containing all related events</returns>
        public OkObjectResult getAllUserEvents()
        {
            return null;
        }

        public OkObjectResult importExistingCalendar()
        {
            return null;
        }

        public OkObjectResult getUsersFriends()
        {
            return null;
        }

        /// <summary>
        /// Given the groupID this will return all groups associated with them
        /// </summary>
        /// <returns>Json result of every group event</returns>
        [HttpGet]
        public OkObjectResult getAllGroupEvents(int groupID)
        {
            var group = _context.Groups
                .Include(g => g.Events)
                .Include(g => g.Users)
                .Where(g => g.GroupID == groupID)
                .Single();

            var events = group.Events.Select(e => new 
            { 
                StartTime = e.StartTime,
                Members = group.Users.Select(u => new 
                { 
                    firstName = u.FirstName
                })
            });

            return Ok(JsonConvert.SerializeObject(events));
        }

        /// <summary>
        /// This is used when needing to check if the endpoints can be hit without worrying about other items
        /// </summary>
        /// <returns>simple Hello Json object</returns>
        [HttpGet]
        public OkObjectResult testEndpoint()
        {
            return Ok(new { Hi = "hello" });
        }

        public OkObjectResult addFriend(int friendCode)
        {
            return null;
        }

        public OkObjectResult addFriend(int friendCode, int userCode)
        {
            return null;
        }

        public OkObjectResult removeFriend(int friendCode)
        {
            return null;
        }

        public OkObjectResult removeFriend(int friendCode, int usercode)
        {
            return null;
        }
        /// <summary>
        /// Restrict this to a single group for the moment for complexity sake
        /// </summary>
        /// <returns></returns>
        public OkObjectResult findTime()
        {
            return null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
