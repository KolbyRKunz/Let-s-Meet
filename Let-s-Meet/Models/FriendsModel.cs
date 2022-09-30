using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Let_s_Meet.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FriendsModel
    {
        [Key]
        [JsonProperty(PropertyName = "friendsID")]
        public int FriendsID { get; set; }

        [JsonProperty(PropertyName = "requestedByID")]
        public int RequestedByID { get; set; }

        [JsonProperty(PropertyName = "requestedToID")]
        public int RequestedToID { get; set; }

        [JsonProperty(PropertyName = "requestedBy")]
        public UserModel RequestedBy { get; set; }

        [JsonProperty(PropertyName = "requestedTo")]
        public UserModel RequestedTo { get; set; }

        [JsonProperty(PropertyName = "requestStatus")]
        public FriendRequestStatus RequestStatus { get; set; }
    }

    public enum FriendRequestStatus
    {
        Sent,
        Accepted,
        Rejected
    }
}
