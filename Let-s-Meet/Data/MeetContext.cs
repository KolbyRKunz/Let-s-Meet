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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventModel>().ToTable("Event");
            modelBuilder.Entity<GroupModel>().ToTable("Group");
            modelBuilder.Entity<UserModel>().ToTable("User");
        }
    }
}