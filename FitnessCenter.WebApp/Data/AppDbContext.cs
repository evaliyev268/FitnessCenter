using Microsoft.EntityFrameworkCore;
using FitnessCenter.WebApp.Models;

namespace FitnessCenter.WebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Price için decimal precision
            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18,2)");

            // Foreign Key Cascade ayarları
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Trainer)
                .WithMany()
                .HasForeignKey(a => a.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}