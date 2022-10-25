using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Let_s_Meet.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserModel
    {
        [Key]
        [JsonProperty(PropertyName = "userID")]
        public int UserID { get; set; }  //Primary Key

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }   

        //TODO: add more columns that we want a user to have



        [JsonProperty(PropertyName = "groups")]
        public ICollection<GroupModel> Groups { get; set; }

        [JsonProperty(PropertyName = "friends")]
        public ICollection<UserModel> Friends { get; set; }

        [JsonProperty(PropertyName = "events")]
        public ICollection<EventModel> Events { get; set; }

        [JsonProperty(PropertyName = "calendars")]
        public ICollection<CalendarModel> Calendars { get; set; }

        [JsonProperty(PropertyName = "settings")]
        public SettingsModel settings { get; set; }
    }
}
