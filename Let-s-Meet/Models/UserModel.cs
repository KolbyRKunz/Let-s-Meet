using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
//TODO: put in identity and make this more offical 

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

        //TODO: add more columns that we want a user to have



        [JsonProperty(PropertyName = "groups")]
        public ICollection<GroupModel> GroupModels { get; set; }

        [JsonProperty(PropertyName = "friends")]
        public ICollection<UserModel> Friends { get; set; }

        [JsonProperty(PropertyName = "events")]
        public ICollection<EventModel> EventModels { get; set; }


    }
}
