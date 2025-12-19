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

                // 1. Veritabaný yoksa oluþtur
                context.Database.EnsureCreated();

                // 2. Admin Kontrolü (Senin Numaranla)
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
                }

                // 3. Eðitmen Kontrolü (Eðer tablo boþsa örnek veri ekle)
                if (!context.Trainers.Any())
                {
                    var trainers = new List<Trainer>
                    {
                        new Trainer
                        {
                            Name = "Yusuf Ziya Gök",
                            Speciality = "Vücut Geliþtirme",
                            ImageUrl = "https://randomuser.me/api/portraits/men/32.jpg"
                        },
                        new Trainer
                        {
                            Name = "Þahin Baðcý",
                            Speciality = "Pilates & Yoga",
                            ImageUrl = "https://randomuser.me/api/portraits/men/59.jpg"
                        },
                        new Trainer
                        {
                            Name = "Okan Koca",
                            Speciality = "Crossfit",
                            ImageUrl = "https://randomuser.me/api/portraits/men/85.jpg"
                        }
                    };

                    context.Trainers.AddRange(trainers);
                }
                if (!context.Services.Any())
                {
                    var services = new List<Service>
                    {
                        new Service
                        {
                            Name = "Fitness Üyeliði (Aylýk)",
                            Description = "Tüm aletlere sýnýrsýz eriþim.",
                            Duration = 30, // Günlük ortalama süre gibi düþünülebilir
                            Price = 1500,
                            ImageUrl = "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=500"
                        },
                        new Service
                        {
                            Name = "Özel Yoga Dersi",
                            Description = "Eðitmen eþliðinde birebir yoga.",
                            Duration = 60,
                            Price = 750,
                            ImageUrl = "https://images.unsplash.com/photo-1599901860904-17e6ed7083a0?w=500"
                        },
                        new Service
                        {
                            Name = "Pilates Reformer",
                            Description = "Aletli pilates ile duruþ bozukluðu düzeltme.",
                            Duration = 50,
                            Price = 900,
                            ImageUrl = "https://images.unsplash.com/photo-1518310383802-640c2de311b2?w=500"
                        }
                    };
                    context.Services.AddRange(services);
                }
                // ---------------------------------------
                // Deðiþiklikleri Kaydet
                context.SaveChanges();
            }
        }
    }
}