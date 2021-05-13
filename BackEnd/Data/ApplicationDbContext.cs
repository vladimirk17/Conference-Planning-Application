using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Забезпечити унікальність імені учасника
            modelBuilder.Entity<Attendee>()
                .HasIndex(a => a.UserName)
                .IsUnique();

            //Зв'язки багато до багатьох, ключі для розв'язувальних таблиць
            modelBuilder.Entity<SessionAttendee>()
                .HasKey(ca => new {ca.SessionId, ca.AttendeeId});

            modelBuilder.Entity<SessionSpeaker>()
                .HasKey(ss => new {ss.SessionId, ss.SpeakerId});
        }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
    }
}