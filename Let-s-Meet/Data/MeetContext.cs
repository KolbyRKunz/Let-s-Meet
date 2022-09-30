using Let_s_Meet.Models;
using Microsoft.EntityFrameworkCore;

namespace Let_s_Meet.Data
{
    public class MeetContext : DbContext
    {
        public MeetContext(DbContextOptions<MeetContext> options) : base(options)
        {
        }

        public DbSet<EventModel> Events { get; set; }
        public DbSet<GroupModel> Groups { get; set; }
        public DbSet<UserModel> Users { get; set; }  //Tempory value. Identity will take care of this later
        public DbSet<OnboardingModel> Onboarding { get; set; }
        public DbSet<EventPromptModel> EventPrompt { get; set; }
        public DbSet<CommentsModel> Comments { get; set; }
        public DbSet<AttendanceModel> Attendance { get; set; }
        public DbSet<FriendsModel> Friends { get; set; }
        public DbSet<CalendarModel> Calendars { get; set; }
        public DbSet<SettingsModel> Settings { get; set; }
        public DbSet<CalendarPrivacyModel> CalendarPrivacy { get; set; }
        public DbSet<EventPrivacyModel> EventPrivacy { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventModel>().ToTable("Event");
            modelBuilder.Entity<GroupModel>().ToTable("Group");
            modelBuilder.Entity<UserModel>().ToTable("User");
            modelBuilder.Entity<OnboardingModel>().ToTable("Onboarding");
            modelBuilder.Entity<EventPromptModel>().ToTable("EventPrompt");
            modelBuilder.Entity<CommentsModel>().ToTable("Comments");
            modelBuilder.Entity<AttendanceModel>().ToTable("Attendance");
            modelBuilder.Entity<CalendarModel>().ToTable("Calendar");

            modelBuilder.Entity<FriendsModel>().ToTable("Friends")
                .HasAlternateKey("RequestedByID", "RequestedToID");
            modelBuilder.Entity<FriendsModel>().HasOne(f => f.RequestedBy).WithMany().HasForeignKey("RequestedByID").OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<FriendsModel>().HasOne(f => f.RequestedTo).WithMany().HasForeignKey("RequestedToID").OnDelete(DeleteBehavior.NoAction);

            //.HasAlternateKey(c => new { c.RequestedBy, c.RequestedTo });
            modelBuilder.Entity<SettingsModel>().ToTable("Settings");
            modelBuilder.Entity<CalendarPrivacyModel>().ToTable("CalendarPrivacy");
            modelBuilder.Entity<EventPrivacyModel>().ToTable("EventPrivacy");
        }
    }
}