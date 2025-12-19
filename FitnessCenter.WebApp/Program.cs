<<<<<<< Updated upstream
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
=======
using Microsoft.EntityFrameworkCore;
using FitnessCenter.WebApp.Data; // AppDbContext'i görebilmesi için gerekli

var builder = WebApplication.CreateBuilder(args);

// 1. Veri Tabaný Baðlantýsý (SQL Server)
// appsettings.json dosyasýndaki "DefaultConnection" adresini kullanýr.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Giriþ Yapma (Authentication) Servisi
// Kullanýcý giriþ yaptýðýnda tarayýcýda çerez (cookie) tutmasýný saðlar.
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", config =>
    {
        config.Cookie.Name = "FitnessCenter.Cookie"; // Çerez adý
        config.LoginPath = "/Account/Login"; // Giriþ yapmamýþ kullanýcýyý buraya atar
    });

// MVC Servisleri
>>>>>>> Stashed changes
builder.Services.AddControllersWithViews();

var app = builder.Build();

<<<<<<< Updated upstream
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
=======
// HTTP request pipeline yapýlandýrmasý.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
>>>>>>> Stashed changes
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

<<<<<<< Updated upstream
=======
// 3. Kimlik Doðrulama Sýralamasý (Çok Önemli!)
// Önce kimlik kontrol edilir (Authentication), sonra yetki (Authorization).
app.UseAuthentication();
>>>>>>> Stashed changes
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

<<<<<<< Updated upstream
app.Run();
=======
app.Run();
>>>>>>> Stashed changes
