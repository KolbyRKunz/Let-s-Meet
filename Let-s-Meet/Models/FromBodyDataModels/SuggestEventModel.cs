using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Let_s_Meet.Models.FromBodyDataModels
{
    // Add profile data for application users by adding properties to the User class
    public class SuggestEventModel
    {
        [Required(ErrorMessage = "groupID is required")]
        public int groupID { get; set; }
        [Required(ErrorMessage = "duration is required")]
        public string duration { get; set; }
        [Required(ErrorMessage = "withinDays is required")]
        public int withinDays { get; set; }
        [Required(ErrorMessage = "title is required")]
        public string title { get; set; }
        [Required(ErrorMessage = "location is required")]
        public string location { get; set; }
    }
}
