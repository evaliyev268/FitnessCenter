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
    }
}