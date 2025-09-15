using FitnessBookingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessBookingApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> tbUsers { get; set; } = null!;
        public DbSet<Training> tbTrainings { get; set; } = null!;
        public DbSet<TrainingRegistration> tbTrainingRegistrations { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrainingRegistration>()
                .HasIndex(tr => new { tr.UserId, tr.TrainingId })
                .IsUnique(); // odpovídá UNIQUE(UserId, TrainingId)

            modelBuilder.Entity<TrainingRegistration>()
                .HasOne(tr => tr.User)
                .WithMany(u => u.TrainingRegistrations)
                .HasForeignKey(tr => tr.UserId);

            modelBuilder.Entity<TrainingRegistration>()
                .HasOne(tr => tr.Training)
                .WithMany(t => t.TrainingRegistrations)
                .HasForeignKey(tr => tr.TrainingId);
        }
    }

}
