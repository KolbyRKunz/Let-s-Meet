using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Let_s_Meet.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SettingsModel
    {
        [Key]
        [JsonProperty(PropertyName = "settingsID")]
        public int SettingsID { get; set; }
        
        [JsonProperty(PropertyName = "user")]
        public UserModel User { get; set; }

        public int UserID { get; set; }
    }

}
