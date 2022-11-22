using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Let_s_Meet.Models.FromBodyDataModels
{
    // Add profile data for application users by adding properties to the User class
    public class EmailModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string email { get; set; }
    }
}
