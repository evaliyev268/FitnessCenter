using Microsoft.EntityFrameworkCore;
using FitnessCenter.WebApp.Models;

namespace FitnessCenter.WebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } // Kullanýcýlar tablosu
        public DbSet<Trainer> Trainers { get; set; } // Eðitmenler tablosu
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. "Price" (Fiyat) alaný için SQL ayarý (Uyarýyý gidermek için)
            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18,2)"); // Toplam 18 basamak, virgülden sonra 2 basamak

            // 2. Cascade Delete Çakýþmasýný Önleme (Hata Çözümü)
            // "Bir Eðitmenin bir Hizmeti vardýr, Hizmet silinirse Eðitmeni SÝLME (Restrict)"
            modelBuilder.Entity<Trainer>()
                .HasOne(t => t.Service)
                .WithMany()
                .HasForeignKey(t => t.ServiceId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade yerine Restrict yaptýk

            base.OnModelCreating(modelBuilder);
        }
    }
}