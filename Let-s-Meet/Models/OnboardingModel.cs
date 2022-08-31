using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Let_s_Meet.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class OnboardingModel
    {
        [Key]
        [JsonProperty(PropertyName = "onboardingID")]
        public int OnboardingID { get; set; }  //Primary Key

        [JsonProperty(PropertyName = "userID")]
        public int UserID { get; set; }

        [JsonProperty(PropertyName = "lastStepCompleted")]
        public int LastStepCompleted  { get; set; }

        //TODO: any other columns we would want an onboarding object to have

    }
}
