using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // SelectList için gerekli
using Microsoft.EntityFrameworkCore;
using FitnessCenter.WebApp.Data;
using FitnessCenter.WebApp.Models;
using System.Security.Claims; // Kullanýcý ID'sini bulmak için

namespace FitnessCenter.WebApp.Controllers
{
    // Sadece giriþ yapanlar randevu alabilir
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class AppointmentController : Controller
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        // --- RANDEVU ALMA SAYFASI (GET) ---
        [HttpGet]
        public IActionResult Create()
        {
            // Dropdown (Seçim Kutularý) için verileri hazýrlýyoruz
            ViewBag.Trainers = new SelectList(_context.Trainers, "Id", "Name");
            ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
            return View();
        }

        // --- RANDEVUYU KAYDETME (POST) ---
        [HttpPost]
        public IActionResult Create(Appointment appointment)
        {
            // Giriþ yapan kullanýcýnýn ID'sini bulma (Cookie'den)
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user != null)
            {
                appointment.UserId = user.Id; // Randevuyu bu kullanýcýya ata

                // Basit bir kontrol: Tarih geçmiþte mi?
                if (appointment.AppointmentDate < DateTime.Now)
                {
                    ModelState.AddModelError("", "Geçmiþ bir tarihe randevu alamazsýnýz.");
                }
                else
                {
                    _context.Appointments.Add(appointment);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Home"); // Baþarýlýysa Ana Sayfaya
                }
            }

            // Hata varsa listeleri tekrar yükle
            ViewBag.Trainers = new SelectList(_context.Trainers, "Id", "Name");
            ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
            return View(appointment);
        }
    }
}