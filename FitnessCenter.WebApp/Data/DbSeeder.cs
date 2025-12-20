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

                // ❌ BU SATIRI SİL VEYA YORUM SAT YAP: 
                // context.Database.EnsureCreated();

                // ✅ Migration ile veritabanı oluşturulacak, burada sadece seed data ekleyeceğiz

                // --- ADMIN HESABI KONTROLÜ ---
                string adminEmail = "b231210553@sakarya.edu.tr";
                if (!context.Users.Any(u => u.Email == adminEmail))
                {
                    var admin = new User
                    {
                        FullName = "Admin User",
                        Email = adminEmail,
                        Password = "sau",
                        Role = "Admin"
                    };
                    context.Users.Add(admin);
                    context.SaveChanges();
                }

                // --- EĞİTMENLER (TRAINERS) ---
                if (!context.Trainers.Any())
                {
                    var trainers = new List<Trainer>
                    {
                        new Trainer
                        {
                            Name = "Ahmet Yılmaz",
                            Speciality = "Vücut Geliştirme & Kuvvet Antrenmanı",
                            ImageUrl = "https://images.unsplash.com/photo-1571019614242-c5c5dee9f50b?w=400",
                            Bio = "10 yıllık deneyime sahip sertifikalı bodybuilding antrenörü",
                            IsActive = true
                        },
                        new Trainer
                        {
                            Name = "Elif Kaya",
                            Speciality = "Yoga & Pilates",
                            ImageUrl = "https://images.unsplash.com/photo-1544367567-0f2fcb009e0b?w=400",
                            Bio = "Uluslararası yoga sertifikasına sahip meditasyon uzmanı",
                            IsActive = true
                        },
                        new Trainer
                        {
                            Name = "Mehmet Demir",
                            Speciality = "Crossfit & Fonksiyonel Antrenman",
                            ImageUrl = "https://images.unsplash.com/photo-1567013127542-490d757e51fc?w=400",
                            Bio = "Crossfit Level 2 Trainer, eski milli sporcu",
                            IsActive = true
                        },
                        new Trainer
                        {
                            Name = "Ayşe Çelik",
                            Speciality = "Zumba & Cardio Dance",
                            ImageUrl = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=400",
                            Bio = "Dans eğitmeni ve grup dersleri uzmanı",
                            IsActive = true
                        }
                    };
                    context.Trainers.AddRange(trainers);
                    context.SaveChanges();
                }

                // --- HİZMETLER (SERVICES) ---
                if (!context.Services.Any())
                {
                    var services = new List<Service>
                    {
                        new Service
                        {
                            Name = "Kişisel Antrenman (60 dk)",
                            Description = "Birebir eğitmen eşliğinde özel program",
                            Duration = 60,
                            Price = 350,
                            ImageUrl = "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?w=500",
                            IsActive = true
                        },
                        new Service
                        {
                            Name = "Grup Yoga Dersi (90 dk)",
                            Description = "Haftanın her günü yoga seansları",
                            Duration = 90,
                            Price = 150,
                            ImageUrl = "https://images.unsplash.com/photo-1599901860904-17e6ed7083a0?w=500",
                            IsActive = true
                        },
                        new Service
                        {
                            Name = "Crossfit Bootcamp (45 dk)",
                            Description = "Yüksek yoğunluklu interval antrenmanı",
                            Duration = 45,
                            Price = 200,
                            ImageUrl = "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=500",
                            IsActive = true
                        },
                        new Service
                        {
                            Name = "Beslenme Danışmanlığı (30 dk)",
                            Description = "Kişiselleştirilmiş beslenme planı",
                            Duration = 30,
                            Price = 250,
                            ImageUrl = "https://images.unsplash.com/photo-1490645935967-10de6ba17061?w=500",
                            IsActive = true
                        },
                        new Service
                        {
                            Name = "Zumba Dans (60 dk)",
                            Description = "Eğlenceli kardio ve dans kombinasyonu",
                            Duration = 60,
                            Price = 120,
                            ImageUrl = "https://images.unsplash.com/photo-1518611012118-696072aa579a?w=500",
                            IsActive = true
                        },
                        new Service
                        {
                            Name = "Pilates Reformer (50 dk)",
                            Description = "Aletli pilates ile duruş düzeltme",
                            Duration = 50,
                            Price = 280,
                            ImageUrl = "https://images.unsplash.com/photo-1518310383802-640c2de311b2?w=500",
                            IsActive = true
                        }
                    };
                    context.Services.AddRange(services);
                    context.SaveChanges();
                }

                // --- ÖRNEK ÜYE HESAPLARI ---
                if (context.Users.Count() == 1) // Sadece admin varsa
                {
                    var members = new List<User>
                    {
                        new User
                        {
                            FullName = "Zeynep Yıldız",
                            Email = "zeynep@example.com",
                            Password = "123456",
                            Role = "Member"
                        },
                        new User
                        {
                            FullName = "Can Öztürk",
                            Email = "can@example.com",
                            Password = "123456",
                            Role = "Member"
                        }
                    };
                    context.Users.AddRange(members);
                    context.SaveChanges();
                }
            }
        }
    }
}