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
using System.Text.RegularExpressions;
using System.Globalization;
using System.Dynamic;
using System.Transactions;
using Microsoft.AspNetCore.Identity;
using Let_s_Meet.Areas.Identity.Data;
using System.Xml.Linq;
using System.Reflection.Metadata;

namespace Let_s_Meet.Controllers
{
    public class APIController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MeetContext _context;
        private readonly IdentityContext _identity_context;
        private readonly UserManager<User> _um;

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

        public APIController(ILogger<HomeController> logger, MeetContext context, IdentityContext identity_context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _identity_context = identity_context;
            _um = userManager;
        }

        [Authorize]
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
        /// Create a new group from the given groupName and members in the database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<OkObjectResult> createGroupAsync(string groupName, string groupMembers)
        {
            //TODO:The groupMembers is a string that has the members separated by a comma
            //I couldn't figure out how to pass in an array via http. I figure pass the entire array of group members and let the back end do it
            //IMPORTANT: The groupMembers list from the frontend should include the person creating the group
            //TODO: later on need to make sure that the user exists before adding to group?

            //Add the user creating the group to the memberslist?

            //TODO: Make this much cleaner, right now just doing it like this to get something on the frontend working to figure that out
            User userI = await _um.GetUserAsync(User);
            groupMembers += "," + userI.Email;
            var membersSplit = groupMembers.Split(",");

            var members = new List<UserModel>();

            for (int i = 0; i < membersSplit.Length; i++)//TODO: is there a better way
            {
                var IdentUser = _identity_context.Users
                    .Where(u => u.Email == membersSplit[i])
                    .Single(); //this should be fine, one email should have one account

                UserModel user = _context.Users
                    .Where(u => u.UserID == IdentUser.UserID)
                    .Single();

                members.Add(user);
            }
            
            /*TODO: adding the group to the user model, Groups column?
                Is this taken care of by GroupModelUserModel table
                that is automatically created?*/

            var groupModel = new GroupModel
            {
                GroupName = groupName,
                Users = members
            };

            _context.Add(groupModel);
            _context.SaveChanges();

            return Ok(new { 
                groupName = groupName,
                groupMembers = groupMembers
            });
        }

        [HttpGet]
        public OkObjectResult getGroupMembers(int groupId)
        {
            var group = _context.Groups
                .Include(g => g.Users)
                .Where(g => g.GroupID == groupId)
                .Single();

            var members = group.Users.Select(m => new
            {
                member = m.FirstName //TODO: we might want to return email instead? or make it possible to add somebody as friend from group member list
            });

            return Ok(members);
        }

        /// <summary>
        /// Given the groupID this will return all groups associated with them
        /// </summary>
        /// <returns>Json result of every group event</returns>
        //[HttpGet]
        //public OkObjectResult getGroupEventsQueryVersion(int groupID)
        //{
        //    var group = _context.Groups
        //        .Include(g => g.Events)
        //        .Include(g => g.Users)
        //        .Where(g => g.GroupID == groupID)
        //        .Single();

        //    var events = group.Events.Select(e => new 
        //    { 
        //        StartTime = e.StartTime,
        //        Members = group.Users.Select(u => new 
        //        { 
        //            firstName = u.FirstName
        //        })
        //    });

        //    return Ok(JsonConvert.SerializeObject(events));
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        [HttpGet]
        public OkObjectResult getGroupEvents(int groupID)
        {
            var userEvents = new[] {
                new {
                    id = "1", //FullCalendar wants it as a String not int
                    title = "Event1",
                    start = DateTime.UtcNow.AddHours(0).ToString("O"),
                    end = DateTime.UtcNow.AddHours(3).ToString("O")
                },
                new{
                    id = "2", //FullCalendar wants it as a String not int
                    title = "Event2",
                    start = DateTime.UtcNow.AddHours(28).ToString("O"),
                    end = DateTime.UtcNow.AddHours(31).ToString("O")
                }
            };

            return Ok(userEvents);
        }

        /// <summary>
        /// Finds all of a given User's events for the purpose of displaying on
        /// their individual calendar
        /// </summary>
        /// <param name=""></param>
        /// <returns>A given users events in the time frame in Json</returns>
        [HttpGet]
        public OkObjectResult getUserEvents(string name)
        {
            return Ok(null); // TODO handle error from "var user = ..." line or ignore
            
            //TODO: is there a better way to do this than querying database
            //Is there a way to get the index to pass in the UserID somehow
            User userIDFromName = _identity_context.Users
                .Where(u => u.UserName == name)
                .Single(); //This should be fine since each email should only have 1 account attached

            var user = _context.Users
                .Include(e => e.Events)
                .Where(u => u.UserID == userIDFromName.UserID)
                .Single();

            var events = user.Events
                .Select(e => new {
                    id = e.EventID.ToString(), //FullCalendar wants it as string
                    title = e.Title,
                    start = e.StartTime.ToString("O"), //full calendar wants it as a ISO8601 format and need to use the datetime tostring argument for that
                    end = e.EndTime.ToString("O"),
                });

            return Ok(events);
        }

        /// <summary>
        /// Removes a given event from the user's event
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpDelete]
        public OkObjectResult deleteUserEvent(string name, int eventID)
        {
            //TODO: Is the userID needed here? eventID should be primary key and unique enough to delete the event
            //TODO: Need to error check incase something that doesn't exist tries to get deleted
            var eventToDelete = _context.Events
                .Include(u => u.Users)
                .Where(e => e.EventID == eventID)
                .Single(); //single should be alright, if not probably a bigger problem in how eventIds are being assigned

            _context.Remove(eventToDelete);
            _context.SaveChanges();

            return Ok(eventToDelete);
        }
        
        /// <summary>
        /// Adds a given event to the user's event data
        /// startTime and endTime need to be a string in UTC format
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public OkObjectResult addUserEvent(string name, string title, string startTime, string endTime)
        {
            //TODO: is there a better way to do this than querying database
            //Is there a way to get the index to pass in the UserID somehow
            User userIDFromName = _identity_context.Users
                .Where(u => u.UserName == name)
                .Single(); //This should be fine since each email should only have 1 account attached

            UserModel user = _context.Users
                .Include(e => e.Events)
                .Where(u => u.UserID == userIDFromName.UserID) //TODO: need to change the 0
                .Single();

            var eventsConnectedUser = new List<UserModel>();
            eventsConnectedUser.Add(user);

            var eventModel = new EventModel
            {
                StartTime = Convert.ToDateTime(startTime), //Sep 16 2022 04:30:00 GMT this is the format needed for this method, date, time, timezone
                EndTime = Convert.ToDateTime(endTime),   //This way causes it to throw errors outside of postman
                Title = title,
                Users = eventsConnectedUser
            };

            _context.Add(eventModel);
            _context.SaveChanges();   //Do this instead? _context.SaveChangesAsync();

            return Ok(new { StartTime = Convert.ToDateTime(startTime), EndTime = Convert.ToDateTime(endTime), Title = title, userId = name }); //TODO: fix what is returned since I'm not sure what it's supposed to return really
        }

        [HttpGet]
        public OkObjectResult getUserGroups(string name)
        {
            var userIDfromName = _identity_context.Users
                .Where(u => u.UserName == name)
                .Single(); //should be fine only one user account per email?

            var userGroups = _context.Users
                .Include(g => g.Groups)
                .Where(g => g.UserID == userIDfromName.UserID)
                .Single();

            var groups = userGroups.Groups
                .Select(g => new { 
                    groupName = g.GroupName,
                    groupId = g.GroupID
                });;

            return Ok(groups);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        public OkObjectResult getUserFriends(int userID)
        {
            var userFriends = new[] {
                new {
                    firstName = "Friend",
                    lastName = "A"
                },
                new{
                    firstName = "Friend",
                    lastName = "B"
                },
                new{
                    firstName = "Friend",
                    lastName = "C"
                },
            };

            return Ok(userFriends);
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
        public OkObjectResult findTime(int groupID, string title)
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
