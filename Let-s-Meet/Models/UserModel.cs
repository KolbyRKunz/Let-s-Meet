using System;
using Newtonsoft.Json;
//TODO: put in identity and make this more offical 

namespace Let_s_Meet.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserModel
    {
        [JsonProperty(PropertyName = "ID")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "FirstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "LastName")]
        public string LastName { get; set; }


    }
}
