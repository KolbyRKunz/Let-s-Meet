using System;
using Newtonsoft.Json;

namespace Let_s_Meet.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class EventModel
    {
        [JsonProperty(PropertyName = "ID")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "startTime")]
        public DateTime startTime { get; set; }

        [JsonProperty(PropertyName = "endTime")]
        public DateTime endTime { get; set; }

        [JsonProperty(PropertyName = "Group")]
        public GroupModel Group { get; set; }

    }
}
