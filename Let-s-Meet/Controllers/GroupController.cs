using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using QRCoder;
using System.Drawing;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Security.Cryptography;
using Image = SixLabors.ImageSharp.Image;

namespace Let_s_Meet.Controllers
{
    public class GroupController : Controller
    {
        public IActionResult Index(string joinCode)
        {
            var fullUrl = this.Url.Action("JoinGroup", "GroupModels", new { joinCode = joinCode }, "https");
            var fullUrlRedirect = this.Url.Action("JoinGroupRedirect", "GroupModels", new { joinCode = joinCode }, "https");
            Regex r = new Regex(@"^*\d");
            int groupID = -1;
            try
            {
                groupID = int.Parse(r.Match(joinCode).Value);
            } catch {
                return Ok(new { status = "error", message = "Failed to Idenitify Group" });
            }
            ViewBag.GroupId = groupID;
            ViewBag.fullUrlRedirect = fullUrlRedirect;
            //retreive group name and put it in the viewbag
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
