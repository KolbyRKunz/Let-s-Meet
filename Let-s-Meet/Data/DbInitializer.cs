using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Let_s_Meet.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MeetContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return; // if there is anthing already in the db return
            }

            //TODO: create mock data to put into the DB here and save it to the context

            //if you create the data in an array then you can do the following.

            //foreach u in users:
            //context.users.add(u);

            // and so on

        }
    }
}
