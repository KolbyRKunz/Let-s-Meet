using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Let_s_Meet.Models.FromBodyDataModels
{
    // Add profile data for application users by adding properties to the User class
    public class CreateCalendarModel
    {
        [Required(ErrorMessage = "title is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "location is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "startTime is required")]
        public string Color { get; set; }
    }
}
