using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Let_s_Meet.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Let_s_Meet.Data
{
    public class IdentityInit
    {
       public static void Initialize(IdentityContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
