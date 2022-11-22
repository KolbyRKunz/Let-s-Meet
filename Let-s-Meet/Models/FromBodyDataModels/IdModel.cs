using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Let_s_Meet.Models.FromBodyDataModels
{
    // Add profile data for application users by adding properties to the User class
    public class IdModel
    {
        [Required(ErrorMessage = "id is required")]
        public int id { get; set; }
    }
}
