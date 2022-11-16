using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Let_s_Meet.Models
{
    // Add profile data for application users by adding properties to the User class
    public class EmailModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string email { get; set; }
    }
}
