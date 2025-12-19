using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FitnessCenter.WebApp.Data;
using FitnessCenter.WebApp.Models;
using System.Linq;

namespace FitnessCenter.WebApp.Controllers
{
    // BU SATIR ÇOK ÖNEMLÝ: Sadece Rolü 'Admin' olanlar buraya girebilir!
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // Admin Ana Sayfasý (Dashboard)
        public IActionResult Index()
        {
            return View();
        }

        // Tüm Üyeleri Listeleme Sayfasý
        public IActionResult Users()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // Eðitmenleri Listeleme Sayfasý (Burada ekle/sil yapacaðýz)
        public IActionResult Trainers()
        {
            var trainers = _context.Trainers.ToList();
            return View(trainers);
        }
        // --- EÐÝTMEN EKLEME (Sayfayý Getir) ---
        [HttpGet]
        public IActionResult CreateTrainer()
        {
            return View();
        }

        // --- EÐÝTMEN EKLEME (Kaydet) ---
        [HttpPost]
        public IActionResult CreateTrainer(Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                _context.Trainers.Add(trainer);
                _context.SaveChanges();
                return RedirectToAction("Trainers");
            }
            return View(trainer);
        }

        // --- EÐÝTMEN SÝLME ---
        [HttpPost]
        public IActionResult DeleteTrainer(int id)
        {
            var trainer = _context.Trainers.Find(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                _context.SaveChanges();
            }
            return RedirectToAction("Trainers");
        }

        // Üye Silme Ýþlemi
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Users");
        }
        // --- HÝZMET LÝSTELEME ---
        public IActionResult Services()
        {
            var services = _context.Services.ToList();
            return View(services);
        }

        // --- HÝZMET EKLEME (Sayfa) ---
        [HttpGet]
        public IActionResult CreateService()
        {
            return View();
        }

        // --- HÝZMET EKLEME (Ýþlem) ---
        [HttpPost]
        public IActionResult CreateService(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(service);
                _context.SaveChanges();
                return RedirectToAction("Services");
            }
            return View(service);
        }

        // --- HÝZMET SÝLME ---
        [HttpPost]
        public IActionResult DeleteService(int id)
        {
            var service = _context.Services.Find(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                _context.SaveChanges();
            }
            return RedirectToAction("Services");
        }

    }
}