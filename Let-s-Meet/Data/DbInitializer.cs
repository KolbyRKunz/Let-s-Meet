using Let_s_Meet.Models;
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


            // Create some users
            List<UserModel> users = CreateUsers(context);

            // Create some groups
            List<GroupModel> groups = CreateGroups(context, users);

            // Create some events
            List<EventModel> events = CreateEvents(context, groups);

            //Create some onboarding objects
            List<OnboardingModel> onboardingModels = CreateOnboarding(context, users);

            //Create some event prompts
            List<EventPromptModel> eventPrompts = CreateEventPrompts(context, users, events);

            //Create some comments
            List<CommentsModel> comments = CreateComments(context, users, events);

            context.SaveChanges();

        }

        /// <summary>
        /// Creates some events and adds them to the database
        /// </summary>
        /// <param name="context"></param>
        /// <param name="groups"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static List<EventModel> CreateEvents(MeetContext context, List<GroupModel> groups)
        {
            List<EventModel> events = new List<EventModel>();

            // Create an event per group
            for (int i = 0; i < groups.Count; i++)
            {
                // Create event
                EventModel eventModel = new EventModel
                {
                    //Group = groups[i],
                    //Title = "Event " + i,
                    //Description = "Description " + i,
                    StartTime = DateTime.Now.AddDays(i),
                    EndTime = DateTime.Now.AddDays(i).AddHours(2),
                    //Location = "Location " + i
                };

                // Add event to list
                events.Add(eventModel);
            }

            // Add events to DB
            context.Events.AddRange(events);

            return events;
        }

        /// <summary>
        /// Creates some groups with users and adds them to the DB
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        private static List<GroupModel> CreateGroups(MeetContext context, List<UserModel> users)
        {
            List<GroupModel> groupModels = new List<GroupModel>();

            int numGroups = 4;

            // Create some groups
            for (int i = 0; i < numGroups; i++)
            {
                HashSet<UserModel> groupMembers = new HashSet<UserModel>();

                // Add some random users to the group
                for (int j = 0; j < 3; j++)
                {
                    int randomUserIndex = new Random().Next(users.Count);
                    groupMembers.Add(users[randomUserIndex]);
                }

                // Create the group
                GroupModel group = new GroupModel()
                {
                    //Name = "Group " + i,
                    //Description = "This is group " + i,
                    Users = groupMembers
                };

                // Add the group to the list of groups
                groupModels.Add(group);
            }

            // Add the groups to the DB
            context.Groups.AddRange(groupModels);

            return groupModels;
        }

        /// <summary>
        /// Creates users and adds them to the DB
        /// </summary>
        /// <param name="context"></param>
        public static List<UserModel> CreateUsers(MeetContext context)
        {
            List<UserModel> users = new List<UserModel>();

            string[] firstNames = new string[] { "John", "Jane", "Mary", "Mark", "Tom" };
            string[] lastNames = new string[] { "Smith", "Johnson", "Williams", "Jones", "Brown" };

            // For each name, add a new user
            for (int i = 0; i < firstNames.Length; i++)
            {
                users.Add(new UserModel
                {
                    FirstName = firstNames[i],
                    LastName = lastNames[i],
                    //Email = firstNames[i].ToLower() + lastNames[i].ToLower() + "@gmail.com",
                    //Password = "password"
                });
            }

            // Add the users to the DB
            context.Users.AddRange(users);

            return users;
        }

        /// <summary>
        /// Creates onboarding objects and adds them to the DB
        /// </summary>
        /// <param name="context"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        public static List<OnboardingModel> CreateOnboarding(MeetContext context, List<UserModel> users)
        {
            List<OnboardingModel> onboarding = new List<OnboardingModel>();

            foreach (UserModel user in users)
            {
                //All users start at step 1 of onboarding process, so their last step completed is 0
                //since they haven't onboarded yet
                onboarding.Add(new OnboardingModel
                {
                    UserID = user.UserID,
                    LastStepCompleted = 0
                });
            }

            //Add the onboarding objects to the DB
            context.Onboarding.AddRange(onboarding);
            return onboarding;
        }

        /// <summary>
        /// Creates event prompts and adds them to the DB
        /// </summary>
        /// <param name="context"></param>
        /// <param name="users"></param>
        /// <param name="events"></param>
        /// <returns></returns>
        public static List<EventPromptModel> CreateEventPrompts(MeetContext context, List<UserModel> users, List<EventModel> events)
        {
            List<EventPromptModel> eventPrompts = new List<EventPromptModel>();

            //Create one event prompt for first user and first event
            eventPrompts.Add(new EventPromptModel
            {
                UserID = users.First().UserID,
                EventID = events.First().EventID
            });

            //Add the onboarding objects to the DB
            context.EventPrompt.AddRange(eventPrompts);
            return eventPrompts;
        }

        /// <summary>
        /// Creates comments and adds them to the DB
        /// </summary>
        /// <param name="context"></param>
        /// <param name="users"></param>
        /// <param name="events"></param>
        /// <returns></returns>
        public static List<CommentsModel> CreateComments(MeetContext context, List<UserModel> users, List<EventModel> events)
        {
            List<CommentsModel> comments = new List<CommentsModel>();

            //Create one comment for first user and first event
            comments.Add(new CommentsModel
            {
                UserID = users.First().UserID,
                EventID = events.First().EventID,
                Time = DateTime.Now
            });

            //Add the onboarding objects to the DB
            context.Comments.AddRange(comments);
            return comments;
        }
    }
}
