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
using Let_s_Meet.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Let_s_Meet.Models.JWTModels;

namespace Let_s_Meet.Controllers
{
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration _configuration;
        private readonly MeetContext _context;

        public AuthController(UserManager<User> userManager, IConfiguration config, MeetContext context)
        {
            this.userManager = userManager;
            this._configuration = config;
            this._context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.Name != null) //don't let user access log in or register once logged in? make them logout first?
            {
                return new RedirectResult("/Home");
            }

            return View();
        }

        [Route("Register")]
        public IActionResult Register()
        {
            if (User.Identity.Name != null) //don't let user access log in or register once logged in? make them logout first?
            {
                return new RedirectResult("/Home");
            }

            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {       
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                var writtenToken = new JwtSecurityTokenHandler().WriteToken(token);

                HttpContext.Session.SetString("Token", writtenToken);

                return Ok(new
                {
                    token = writtenToken,
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [Route("LogOut")]
        public RedirectResult LogOut()
        {
            HttpContext.Session.Remove("Token");
            return new RedirectResult("/Auth");
        }

        //Could use addition of adding password parameters
        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            var userEmail = await userManager.FindByEmailAsync(model.Email);
            if (userEmail != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User with that Email already exists" });

            UserModel MeetUser = new UserModel { FirstName = model.FirstName, LastName = model.LastName, Email = model.Email};
            _context.Add(MeetUser);
            _context.SaveChanges();

            var IdentityUser = new User { UserName = model.Username, Email = model.Email, UserID = MeetUser.UserID };
            var result = await userManager.CreateAsync(IdentityUser, model.Password);
            if (!result.Succeeded)
            {
                var message = new StringBuilder();
                foreach(var error in result.Errors)
                {
                    message.Append(error.Description + ' ');
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = message.ToString() });
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

    }
}
