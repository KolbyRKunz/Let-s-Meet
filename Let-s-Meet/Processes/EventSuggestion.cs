using Itenso.TimePeriod;
using Let_s_Meet.Data;
using Let_s_Meet.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Let_s_Meet.Processes
{
    /// <summary>
    /// Class that provides tools for suggesting events for groups.
    /// This class utilizes the TimePeriod library (docs: https://www.codeproject.com/Articles/168662/Time-Period-Library-for-NET)
    /// </summary>
    public class EventSuggestion
    {
        /// <summary>
        /// Returns a list of events that are free for all users in the group.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="groupID"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="title"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        private static async Task<List<EventModel>> SuggestEvents(MeetContext context, int groupID, TimeSpan duration, DateTime start, DateTime end, string title, string location)
        {
            // Get all users in the group
            var users = await context
                .Groups
                .Include(g => g.Users)
                .Where(g => g.GroupID == groupID)
                .Select(g => g.Users)
                .FirstOrDefaultAsync();

            // Get all events for all users in the group within [start,end]
            List<EventModel> events = await context.Events
                .Include(e => e.Users)
                .Where(e => e.Users.Any(u => users.Contains(u)))
                .Where(e => e.StartTime >= start && e.EndTime <= end)
                .ToListAsync();

            // Add all events to a list of time periods
            TimePeriodCollection eventPeriods = new TimePeriodCollection();
            foreach (EventModel e in events) {

                // Create TimeRange
                TimeRange timeRange = new TimeRange(e.StartTime, e.EndTime);

                // Add TimeRange to event collection
                eventPeriods.Add(timeRange);

            }

            // Combine events to find unavailable times
            TimePeriodCombiner<TimeRange> periodCombiner = new TimePeriodCombiner<TimeRange>();
            ITimePeriodCollection unavailablePeriods = periodCombiner.CombinePeriods(eventPeriods);

            // Create TimePeriod for where to find available times
            TimePeriodCollection range = new TimePeriodCollection();
            range.Add(new TimeRange(start, end));

            // Subtract unavailable from range to get available time periods
            TimePeriodSubtractor<TimeRange> periodSubtractor = new TimePeriodSubtractor<TimeRange>();
            ITimePeriodCollection availablePeriods = periodSubtractor.SubtractPeriods(range, unavailablePeriods);

            // Convert available periods into events
            List<EventModel> availableEvents = new List<EventModel>();
            foreach (TimeRange t in availablePeriods)
            {
                // If the available time is shorter than the duration, skip it
                if (t.Duration < duration) continue;

                // Split the available time into multiple events
                int numEvents = (int)(t.Duration.TotalMinutes / duration.TotalMinutes);
                for (int i = 0; i < numEvents; i++)
                {
                    // Create event
                    EventModel e = new EventModel()
                    {
                        Title = title,
                        Location = location,
                        StartTime = t.Start.AddMinutes(i * duration.TotalMinutes),
                        EndTime = t.Start.AddMinutes((i + 1) * duration.TotalMinutes)
                    };

                    // Add event to list
                    availableEvents.Add(e);
                }
            }
            
            return availableEvents;
        }

        /// <summary>
        /// Runs SuggestEvents on a new thread.
        /// 
        /// Returns a list of events that are free for all users in the group.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="groupID"></param>
        /// <param name="duration"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="title"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static async Task<List<EventModel>> SuggestEventsAsync(MeetContext context, int groupID, TimeSpan duration, DateTime start, DateTime end, string title, string location)
        {
            return await Task.Run(() => SuggestEvents(context, groupID, duration, start, end, title, location));
        }
    }
}
