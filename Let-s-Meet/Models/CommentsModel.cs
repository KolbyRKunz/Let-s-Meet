using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Let_s_Meet.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CommentsModel
    {
        [Key]
        [JsonProperty(PropertyName = "commentID")]
        public int CommentID { get; set; }  //Primary Key

        [JsonProperty(PropertyName = "userID")]
        public int UserID { get; set; }

        [JsonProperty(PropertyName = "eventID")]
        public int EventID { get; set; }

        [JsonProperty(PropertyName = "time")]
        public DateTime Time { get; set; }

        //TODO: any other columns we would want an onboarding object to have

    }
}
