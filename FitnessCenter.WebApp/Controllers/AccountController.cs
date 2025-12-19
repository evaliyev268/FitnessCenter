using Microsoft.AspNetCore.Mvc;
using FitnessCenter.WebApp.Models;
using FitnessCenter.WebApp.Data;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace FitnessCenter.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        // Veri tabanı bağlantısını buraya çağırıyoruz (Dependency Injection)
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // --- KAYIT OL (REGISTER) ---
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. E-posta daha önce alınmış mı kontrol et
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("", "Bu e-posta adresi zaten kayıtlı.");
                    return View(model);
                }

                // 2. Yeni kullanıcıyı oluştur
                var newUser = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password // Not: Gerçek projede şifreler hashlenmelidir!
                };

                // 3. Veri tabanına ekle ve kaydet
                _context.Users.Add(newUser);
                _context.SaveChanges();

                return RedirectToAction("Login"); // Kayıttan sonra giriş sayfasına git
            }
            return View(model);
        }

        // --- GİRİŞ YAP (LOGIN) ---
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // 1. Kullanıcıyı veri tabanında ara
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // 2. Kullanıcı bulunduysa, kimlik bilgilerini hazırla (Cookie oluşturma)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var userIdentity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(userIdentity);

                // 3. Sisteme giriş yap
                await HttpContext.SignInAsync("CookieAuth", principal);

                return RedirectToAction("Index", "Home");
            }

            // Hatalı giriş
            ViewBag.Error = "E-posta veya şifre hatalı.";
            return View();
        }

        // --- ÇIKIŞ YAP (LOGOUT) ---
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }
    }
}