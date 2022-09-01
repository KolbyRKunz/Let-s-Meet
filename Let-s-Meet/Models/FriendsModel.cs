using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Let_s_Meet.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FriendsModel
    {
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
