using System;

namespace Let_s_Meet.Models
{
    public class EventModel
    {
        public int EventId { get; set; }

        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }

        public GroupModel Group { get; set; }

    }
}
