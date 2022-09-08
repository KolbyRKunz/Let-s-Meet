using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Let_s_Meet.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class EventModel
    {
        [Key]
        [JsonProperty(PropertyName = "eventID")]
        public int EventID { get; set; }     //Primary key

        [JsonProperty(PropertyName = "startTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty(PropertyName = "endTime")]
        public DateTime EndTime { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "privacy")]
        public EventPrivacyModel Privacy { get; set; }

        [JsonProperty(PropertyName = "users")]
        public ICollection<UserModel> Users { get; set; }

        [JsonProperty(PropertyName = "group")]
        public GroupModel Group { get; set; }

        [JsonProperty(PropertyName = "calendar")]
        public CalendarModel Calendar { get; set; }

        //TODO: any other columns we would want an event to have

    }
}
