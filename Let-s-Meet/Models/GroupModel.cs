using System;
using System.Collections.Generic;

namespace Let_s_Meet.Models
{
    public class GroupModel
    {
        public int ID { get; set; }

        public int GroupSize { get; set; }

        public ICollection<UserModel> Users { get; set; }

    }
}
