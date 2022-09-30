using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Let_s_Meet.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CalendarPrivacyModel
    {
        [Key]
        [JsonProperty(PropertyName = "calendarPrivacyID")]
        public int CalendarPrivacyID { get; set; }

        [JsonProperty(PropertyName = "calendar")]
        public CalendarModel Calendar { get; set; }

        public int CalendarID { get; set; }

    }

}
