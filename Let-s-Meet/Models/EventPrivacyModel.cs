using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Let_s_Meet.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class EventPrivacyModel
    {
        [Key]
        [JsonProperty(PropertyName = "calendarPrivacyID")]
        public int EventPrivacyID { get; set; }

        [JsonProperty(PropertyName = "event")]
        public EventModel Event { get; set; }

        public int EventId { get; set; }
    }

}
