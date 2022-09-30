using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
//TODO: put in identity and make this more offical 

namespace Let_s_Meet.Models.JWTModels
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

}
