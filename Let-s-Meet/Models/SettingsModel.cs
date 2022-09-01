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
        [JsonProperty(PropertyName = "user")]
        public UserModel User { get; set; }

        public int UserID { get; set; }
    }

}
