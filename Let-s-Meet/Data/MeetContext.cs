using Let_s_Meet.Models;
using Microsoft.EntityFrameworkCore;

namespace Let_s_Meet.Data
{
    public class MeetContext : DbContext
    {
        public MeetContext(DbContextOptions<MeetContext> options) : base(options)
        {
        }

        public DbSet<EventModel> Courses { get; set; }
        public DbSet<GroupModel> Enrollments { get; set; }
        public DbSet<UserModel> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventModel>().ToTable("Event");
            modelBuilder.Entity<GroupModel>().ToTable("Group");
            modelBuilder.Entity<UserModel>().ToTable("User");
        }
    }
}