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
        public int GroupSize { get; set; } // TODO this likely isn't necessary since we can just get the length of the list of users

        [JsonProperty(PropertyName = "Users")]
        public ICollection<UserModel> Users { get; set; }

        // TODO have fields like name, description, owner, and members?

    }
}
