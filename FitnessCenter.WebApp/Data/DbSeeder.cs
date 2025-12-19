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

                // Deðiþiklikleri Kaydet
                context.SaveChanges();
            }
        }
    }
}