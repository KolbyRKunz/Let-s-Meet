using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Let_s_Meet.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GroupModel
    {
        [Key]
        [JsonProperty(PropertyName = "groupID")]
        public int GroupID { get; set; }  //Primary Key

        [JsonProperty(PropertyName = "groupName")]
        public string GroupName { get; set; }

        [JsonProperty(PropertyName = "groupJoinCode")]
        public string JoinCode { get; set; }

        [JsonProperty(PropertyName = "users")]
        public ICollection<UserModel> Users { get; set; }

        [JsonProperty(PropertyName = "events")]
        public ICollection<EventModel> Events { get; set; }

        //TODO: any other columns we would want an event to have

    }
}
