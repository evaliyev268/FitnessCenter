using FitnessCenter.WebApp.Models;

namespace FitnessCenter.WebApp.Data
{
    public static class DbSeeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Veritabanýný oluþtur
                context.Database.EnsureCreated();

                // 1. Admin Ekle
                string adminEmail = "b231210553@sakarya.edu.tr";
                if (!context.Users.Any(u => u.Email == adminEmail))
                {
                    var admin = new User
                    {
                        FullName = "Sistem Yöneticisi",
                        Email = adminEmail,
                        Password = "sau",
                        Role = "Admin"
                    };
                    context.Users.Add(admin);
                    context.SaveChanges();
                }

                // 2. Hizmetleri Ekle
                if (!context.Services.Any())
                {
                    var services = new List<Service>
                    {
                        new Service { Name = "Fitness", Description="Birebir Fitness", Duration = 60, Price = 1500, ImageUrl="https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=500" },
                        new Service { Name = "Yoga", Description="Rahatlatýcý Yoga", Duration = 50, Price = 800, ImageUrl="https://images.unsplash.com/photo-1599901860904-17e6ed7083a0?w=500" },
                        new Service { Name = "Crossfit", Description="Yüksek Yoðunluklu", Duration = 45, Price = 1200, ImageUrl="https://images.unsplash.com/photo-1517836357463-d25dfeac3438?w=500" }
                    };

                    context.Services.AddRange(services);
                    context.SaveChanges();
                }

                // 3. Eðitmenleri Ekle (Hizmetlere Baðlayarak)
                if (!context.Trainers.Any())
                {
                    var fitness = context.Services.FirstOrDefault(s => s.Name == "Fitness");
                    var yoga = context.Services.FirstOrDefault(s => s.Name == "Yoga");
                    var crossfit = context.Services.FirstOrDefault(s => s.Name == "Crossfit");

                    if (fitness != null && yoga != null && crossfit != null)
                    {
                        var trainers = new List<Trainer>
                        {
                            // Elle ID vermiyoruz, ServiceId'leri baðlýyoruz
                            new Trainer { Name = "Yusuf Ziya Gök", Speciality = "Vücut Geliþtirme", ImageUrl = "https://randomuser.me/api/portraits/men/32.jpg", ServiceId = fitness.Id },
                            new Trainer { Name = "Þahin Baðcý", Speciality = "Pilates & Yoga", ImageUrl = "https://randomuser.me/api/portraits/men/59.jpg", ServiceId = yoga.Id },
                            new Trainer { Name = "Okan Koca", Speciality = "Crossfit", ImageUrl = "https://randomuser.me/api/portraits/men/85.jpg", ServiceId = crossfit.Id }
                        };

                        context.Trainers.AddRange(trainers);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}