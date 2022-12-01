using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using Let_s_Meet.Areas.Identity.Data;
using Let_s_Meet.Data;
using Microsoft.AspNetCore.Identity;
using Let_s_Meet.Models;
using System.Collections.Generic;
using System.Linq;

namespace Let_s_Meet.Controllers
{
    public class GroupController : Controller
    {
        private readonly MeetContext _context;
        private readonly UserManager<User> _um;

        public GroupController(MeetContext context, UserManager<User> um)
        {
            _context = context;
            _um = um;
        }

        public IActionResult Index(string joinCode)
        {
            ViewBag.joinCode = joinCode;
            var fullUrl = this.Url.Action("JoinGroup", "GroupModels", new { joinCode = joinCode }, "https");
            var fullUrlRedirect = this.Url.Action("JoinGroupRedirect", "GroupModels", new { joinCode = joinCode }, "https");
            /*Regex r = new Regex(@"^*\d");
            int groupID = -1;*/

            int groupID = 0;
            string groupName = "";
            int groupCalendarID = 0;
            try
            {
                //groupID = int.Parse(r.Match(joinCode).Value);
                //TODO: Make this database look up a bit cleaner and error check since a non existent group will break this?
                User user = _um.GetUserAsync(User).Result;
                UserModel userModel = _context.Users.Find(user.UserID);
                var group = _context.Groups
                    .Where(g => g.JoinCode == joinCode)
                    .Single();
                groupID = group.GroupID;
                groupName = group.GroupName;
                groupCalendarID = group.CalendarID;
            } catch {
                return Ok(new { status = "error", message = "Failed to Idenitify Group" });
            }
            ViewBag.GroupId = groupID;
            ViewBag.fullUrlRedirect = fullUrlRedirect;
            ViewBag.GroupName = groupName;
            ViewBag.GroupCalendarID = groupCalendarID;
            QRCodeGenerator qrGen = new QRCodeGenerator();
            var qr = qrGen.CreateQrCode(fullUrl, QRCodeGenerator.ECCLevel.L);
            var qrCode = new QRCode(qr);
            var bitmap = qrCode.GetGraphic(5);
            string temp = bitmap.ToBase64String(PngFormat.Instance);
            ViewBag.image = temp;
            return View();
        }
    }
}
