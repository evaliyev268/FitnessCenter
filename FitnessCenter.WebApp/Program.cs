using Microsoft.EntityFrameworkCore;
using FitnessCenter.WebApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1. Veri Tabaný Baðlantýsý
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Giriþ Yapma (Authentication) Servisi
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", config =>
    {
        config.Cookie.Name = "FitnessCenter.Cookie";
        config.LoginPath = "/Account/Login"; // Giriþ yapmamýþ kullanýcýyý buraya atar
    });

// MVC Servisleri
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Hata yönetimi ve HTTPS ayarlarý
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 3. Kimlik ve Yetki Sýralamasý
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 4. Admin Hesabýný ve Verileri Oluþtur (Seeder)
FitnessCenter.WebApp.Data.DbSeeder.Seed(app);

app.Run();