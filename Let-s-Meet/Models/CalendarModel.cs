using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Newtonsoft.Json;

namespace Let_s_Meet.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CalendarModel
    {
        [Key]
        [JsonProperty(PropertyName = "calendarID")]
        public int CalendarID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "color")]
        public Color Color { get; set; }

        [JsonProperty(PropertyName = "owner")]
        public UserModel Owner { get; set; }

        [JsonProperty(PropertyName = "members")]
        public List<UserModel> Members { get; set; }

        [JsonProperty(PropertyName = "events")]
        public List<EventModel> Events { get; set; }

        


    }
}
