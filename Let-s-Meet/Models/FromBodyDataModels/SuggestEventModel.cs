using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Let_s_Meet.Models.FromBodyDataModels
{
    // Add profile data for application users by adding properties to the User class
    public class SuggestEventModel
    {
        [Required(ErrorMessage = "calendarId is required")]
        public int calendarID { get; set; }
        [Required(ErrorMessage = "duration is required")]
        public string duration { get; set; }
        [Required(ErrorMessage = "withinDays is required")]
        public int withinDays { get; set; }
        [Required(ErrorMessage = "title is required")]
        public string title { get; set; }
        [Required(ErrorMessage = "location is required")]
        public int location { get; set; }
    }
}
