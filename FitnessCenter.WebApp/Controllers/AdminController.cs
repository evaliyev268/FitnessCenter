using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using FitnessCenter.WebApp.Data;
using FitnessCenter.WebApp.Models;
using System.Linq;

namespace FitnessCenter.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // --- DASHBOARD ---
        public IActionResult Index() => View();

        // --- LÝSTELEME SAYFALARI ---
        public IActionResult Users()
        {
            return View(_context.Users.ToList());
        }

        public IActionResult Trainers()
        {
            return View(_context.Trainers.ToList());
        }

        public IActionResult Services()
        {
            return View(_context.Services.ToList());
        }

        public IActionResult Appointments()
        {
            var appointments = _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();
            return View(appointments);
        }

        // --- RANDEVU SÝLME ---
        [HttpPost]
        public IActionResult DeleteAppointment(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                _context.SaveChanges();
            }
            return RedirectToAction("Appointments");
        }

        // --- EÐÝTMEN ÝÞLEMLERÝ (GÜNCELLENMÝÞ HALÝ) ---
        [HttpGet]
        public IActionResult CreateTrainer()
        {
            ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult CreateTrainer(Trainer trainer)
        {
            // Validasyon sýrasýnda ImageUrl boþ olsa bile hata vermesin diye modelden hatayý siliyoruz
            ModelState.Remove("ImageUrl");

            if (ModelState.IsValid)
            {
                // 1. Email Kontrolü
                if (_context.Users.Any(u => u.Email == trainer.Email))
                {
                    ModelState.AddModelError("", "Bu e-posta adresi zaten kullanýmda.");
                    ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
                    return View(trainer);
                }

                // 2. RESÝM KONTROLÜ (YENÝ)
                // Eðer kullanýcý resim girmediyse varsayýlan bir resim ata
                if (string.IsNullOrEmpty(trainer.ImageUrl))
                {
                    trainer.ImageUrl = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png";
                }

                // 3. Uzmanlýðý Otomatik Doldur
                var service = _context.Services.Find(trainer.ServiceId);
                trainer.Speciality = service?.Name ?? "Genel";

                // 4. Eðitmeni Kaydet
                _context.Trainers.Add(trainer);

                // 5. Otomatik Kullanýcý Oluþtur
                var newUser = new User
                {
                    FullName = trainer.Name,
                    Email = trainer.Email,
                    Password = "123",
                    Role = "Trainer"
                };
                _context.Users.Add(newUser);

                _context.SaveChanges();
                return RedirectToAction("Trainers");
            }

            ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
            return View(trainer);
        }

        [HttpPost]
        public IActionResult DeleteTrainer(int id)
        {
            var trainer = _context.Trainers.Find(id);
            if (trainer != null)
            {
                // Ýliþkili kullanýcýyý da bulup silebiliriz (Ýsteðe baðlý)
                var user = _context.Users.FirstOrDefault(u => u.Email == trainer.Email);
                if (user != null) _context.Users.Remove(user);

                _context.Trainers.Remove(trainer);
                _context.SaveChanges();
            }
            return RedirectToAction("Trainers");
        }

        // --- HÝZMET ÝÞLEMLERÝ ---
        [HttpGet]
        public IActionResult CreateService() => View();

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

        [HttpGet]
        public IActionResult EditService(int id)
        {
            var service = _context.Services.Find(id);
            if (service == null) return NotFound();
            return View(service);
        }

        [HttpPost]
        public IActionResult EditService(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Update(service);
                _context.SaveChanges();
                return RedirectToAction("Services");
            }
            return View(service);
        }

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

        // --- KULLANICI SÝLME (KENDÝNÝ SÝLME KORUMALI) ---
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var userToDelete = _context.Users.Find(id);
            var currentUserEmail = User.Identity.Name;

            if (userToDelete != null)
            {
                // KENDÝNÝ SÝLME KONTROLÜ
                if (userToDelete.Email == currentUserEmail)
                {
                    TempData["Error"] = "Güvenlik gereði kendi hesabýnýzý silemezsiniz!";
                    return RedirectToAction("Users");
                }

                _context.Users.Remove(userToDelete);
                _context.SaveChanges();
            }
            return RedirectToAction("Users");
        }
    }
}