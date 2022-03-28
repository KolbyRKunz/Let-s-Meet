using System;
using System.Collections.Generic;

namespace Let_s_Meet.Models
{
    public class GroupModel
    {
        public int GroupId { get; set; }

        public int GroupSize { get; set; }

        public IEnumerable<UserModel> Users { get; set; }

    }
}
