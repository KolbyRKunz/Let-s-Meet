using System;
using Newtonsoft.Json;

namespace Let_s_Meet.Models
{
    public class ErrorViewModel
    {
        [JsonProperty(PropertyName = "RequestID")]
        public string RequestId { get; set; }

        [JsonProperty(PropertyName = "ShowRequestID")]
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
