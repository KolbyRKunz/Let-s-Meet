using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Let_s_Meet.Models.FromBodyDataModels
{
    // Add profile data for application users by adding properties to the User class
    public class GroupCreationModel
    {
        [Required(ErrorMessage = "id list is required")]
        public string name { get; set; }
        public List<int> friendIds { get; set; }
    }
}