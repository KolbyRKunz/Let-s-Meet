using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Let_s_Meet.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GroupModel
    {
        [JsonProperty(PropertyName = "ID")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "GroupSize")]
        public int GroupSize { get; set; }

        [JsonProperty(PropertyName = "Users")]
        public ICollection<UserModel> Users { get; set; }

    }
}
