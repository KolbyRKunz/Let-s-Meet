using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Let_s_Meet.Models.FromBodyDataModels
{
    // Add profile data for application users by adding properties to the User class
    public class CreateEventModel
    {
        [Required(ErrorMessage = "title is required")]
        public string title { get; set; }
        [Required(ErrorMessage = "location is required")]
        public string location { get; set; }
        [Required(ErrorMessage = "startTime is required")]
        public string startTime { get; set; }
        [Required(ErrorMessage = "endTime is required")]
        public string endTime { get; set; }
        [Required(ErrorMessage = "calendarID is required")]
        public int calendarID { get; set; }
    }
}
