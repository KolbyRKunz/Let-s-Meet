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
        public async Task<IActionResult> Post(string data)
        {
            return Ok(new { message = ("Hello from web server!!" + data) });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> getUsers(string data)
        {
            var element = _context.Users.Select(u => new
            {
                firstName = u.FirstName,
                lastName = u.LastName,
                groups = u.Groups.Select(g => new 
                { 
                    groupName = g.GroupName,
                    /*members = g.Users.Select(m => new 
                    { 
                        firstName = m.FirstName,
                        lastName = m.LastName
                    })*/
                }),
                /*friends = u.Friends.Select(f => new 
                { 
                    firstName = f.FirstName,
                    lastName = f.LastName
                }),*/
                events = u.Events.Select(e => new 
                { 
                    startTime = e.StartTime,
                    endTime = e.EndTime
                })
            });
            return Ok(JsonConvert.SerializeObject(element));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> getEvents(string data)
        {
            var element = _context.Events.Select(e => new
            {
                startTime = e.StartTime,
                endTime = e.EndTime,
                groups = e.Groups.Select(g => new 
                { 
                    groupName = g.GroupName
                }),
                users = e.Users.Select(u => new 
                { 
                    firstName = u.FirstName,
                    lastName = u.LastName
                })
            }) ;
            return Ok(JsonConvert.SerializeObject(element));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> getGroups(string data)
        {
            var element = _context.Groups.Select(g => new 
            { 
                groupName = g.GroupName,
                members = g.Users.Select(m => new 
                { 
                    firstName = m.FirstName,
                    lastName = m.LastName
                }),
                events = g.Events.Select(e => new 
                { 
                    startTime = e.StartTime,
                    endTime = e.EndTime
                })
            });
            return Ok(JsonConvert.SerializeObject(element));
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> Echo(string data)
        {
            /*var element = "Hello from the web server";
            return Ok(JsonConvert.SerializeObject(element));*/

			return Ok(new { body = "Hello from the web " + data });

			//return Ok(new { message = ("Hello from web server!!" + data) });
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
