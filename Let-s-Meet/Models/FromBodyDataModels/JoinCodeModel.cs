using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Let_s_Meet.Models.FromBodyDataModels
{
    // Add profile data for application users by adding properties to the User class
    public class JoinCodeModel
    {
        [Required(ErrorMessage = "title is required")]
        public string joinCode { get; set; }
        
    }
}
