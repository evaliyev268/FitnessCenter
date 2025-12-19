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

                // --- ADMIN HESABI ---
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

                // --- EĞİTMEN HESAPLARI (User olarak giriş yapabilirler) ---
                if (!context.Users.Any(u => u.Role == "Trainer"))
                {
                    var trainerUsers = new List<User>
                    {
                        new User
                        {
                            FullName = "Ahmet Yılmaz",
                            Email = "ahmet@fitness.com",
                            Password = "123456",
                            Role = "Trainer"
                        },
                        new User
                        {
                            FullName = "Elif Kaya",
                            Email = "elif@fitness.com",
                            Password = "123456",
                            Role = "Trainer"
                        },
                        new User
                        {
                            FullName = "Mehmet Demir",
                            Email = "mehmet@fitness.com",
                            Password = "123456",
                            Role = "Trainer"
                        },
                        new User
                        {
                            FullName = "Ayşe Çelik",
                            Email = "ayse@fitness.com",
                            Password = "123456",
                            Role = "Trainer"
                        }
                    };
                    context.Users.AddRange(trainerUsers);
                    context.SaveChanges();
                }

                // --- EĞİTMENLER (Trainers Tablosu) ---
                if (!context.Trainers.Any())
                {
                    var trainers = new List<Trainer>
                    {
                        new Trainer
                        {
                            Name = "Ahmet Yılmaz",
                            Email = "ahmet@fitness.com",
                            Speciality = "Vücut Geliştirme & Kuvvet Antrenmanı",
                            ImageUrl = "https://images.unsplash.com/photo-1571019614242-c5c5dee9f50b?w=400",
                            Bio = "10 yıllık deneyime sahip sertifikalı bodybuilding antrenörü",
                            IsActive = true,
                            WorkStartTime = new TimeSpan(9, 0, 0),  // 09:00
                            WorkEndTime = new TimeSpan(18, 0, 0),   // 18:00
                            HourlyRate = 500,
                            WorkingDays = "Pazartesi, Çarşamba, Cuma"
                        },
                        new Trainer
                        {
                            Name = "Elif Kaya",
                            Email = "elif@fitness.com",
                            Speciality = "Yoga & Pilates",
                            ImageUrl = "https://images.unsplash.com/photo-1544367567-0f2fcb009e0b?w=400",
                            Bio = "Uluslararası yoga sertifikasına sahip meditasyon uzmanı",
                            IsActive = true,
                            WorkStartTime = new TimeSpan(10, 0, 0),
                            WorkEndTime = new TimeSpan(19, 0, 0),
                            HourlyRate = 400,
                            WorkingDays = "Her gün"
                        },
                        new Trainer
                        {
                            Name = "Mehmet Demir",
                            Email = "mehmet@fitness. com",
                            Speciality = "Crossfit & Fonksiyonel Antrenman",
                            ImageUrl = "https://images.unsplash.com/photo-1567013127542-490d757e51fc?w=400",
                            Bio = "Crossfit Level 2 Trainer, eski milli sporcu",
                            IsActive = true,
                            WorkStartTime = new TimeSpan(8, 0, 0),
                            WorkEndTime = new TimeSpan(17, 0, 0),
                            HourlyRate = 450,
                            WorkingDays = "Pazartesi, Salı, Perşembe"
                        },
                        new Trainer
                        {
                            Name = "Ayşe Çelik",
                            Email = "ayse@fitness.com",
                            Speciality = "Zumba & Cardio Dance",
                            ImageUrl = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=400",
                            Bio = "Dans eğitmeni ve grup dersleri uzmanı",
                            IsActive = true,
                            WorkStartTime = new TimeSpan(14, 0, 0),
                            WorkEndTime = new TimeSpan(21, 0, 0),
                            HourlyRate = 350,
                            WorkingDays = "Salı, Perşembe, Cumartesi"
                        }
                    };
                    context.Trainers.AddRange(trainers);
                    context.SaveChanges();
                }

                // --- HİZMETLER ---
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
                if (context.Users.Count(u => u.Role == "Member") == 0)
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