using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessCenter.WebApp.Data;
using FitnessCenter.WebApp.Models;
using System.Security.Claims;

namespace FitnessCenter.WebApp.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class AppointmentController : Controller
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        // --- 1. RANDEVU ALMA SAYFASI (GET) ---
        [HttpGet]
        public IActionResult Create()
        {
            // Hizmetleri alýyoruz
            var services = _context.Services.ToList();

            // Dropdown için SelectList
            ViewBag.Services = new SelectList(services, "Id", "Name");

            // JavaScript ile Fiyat/Süre göstermek için tüm listeyi de gönderiyoruz
            ViewBag.ServicesList = services;

            // Eðitmenleri ServiceId ile gönderiyoruz (JS Filtrelemesi için)
            var trainers = _context.Trainers.Select(t => new {
                Id = t.Id,
                Name = t.Name,
                ServiceId = t.ServiceId
            }).ToList();

            ViewBag.TrainersList = trainers;

            return View();
        }

        // --- 2. RANDEVUYU KAYDETME (POST) ---
        [HttpPost]
        public IActionResult Create(Appointment appointment)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            // Seçilen hizmetin süresini bul
            var selectedService = _context.Services.Find(appointment.ServiceId);

            if (user != null && selectedService != null)
            {
                appointment.UserId = user.Id;
                appointment.Status = "Bekliyor";

                // A. Geçmiþ Tarih Kontrolü
                if (appointment.AppointmentDate < DateTime.Now)
                {
                    ModelState.AddModelError("", "Geçmiþ bir tarihe randevu alamazsýnýz.");
                }
                else
                {
                    // B. GELÝÞMÝÞ ÇAKIÞMA KONTROLÜ

                    DateTime newStart = appointment.AppointmentDate;
                    DateTime newEnd = newStart.AddMinutes(selectedService.Duration);

                    // Veritabanýndaki "Onaylý" randevularý çek
                    var existingAppointments = _context.Appointments
                        .Include(a => a.Service)
                        .Where(a => a.TrainerId == appointment.TrainerId && a.Status == "Onaylandý")
                        .ToList();

                    // YENÝ MANTIK: Çakýþan randevuyu saklamak için deðiþken
                    Appointment? conflictingAppointment = null;

                    foreach (var existing in existingAppointments)
                    {
                        DateTime existingStart = existing.AppointmentDate;
                        DateTime existingEnd = existingStart.AddMinutes(existing.Service.Duration);

                        // Çakýþma var mý?
                        if (newStart < existingEnd && existingStart < newEnd)
                        {
                            conflictingAppointment = existing; // Çakýþan randevuyu YAKALA
                            break; // Ýlk çakýþmada döngüden çýk
                        }
                    }

                    if (conflictingAppointment != null)
                    {
                        // HATA MESAJINI DÜZELTTÝK:
                        // Artýk kullanýcýnýn seçtiði saati deðil, HOCANIN DOLU OLDUÐU saati gösteriyoruz.
                        DateTime busyStart = conflictingAppointment.AppointmentDate;
                        DateTime busyEnd = busyStart.AddMinutes(conflictingAppointment.Service.Duration);

                        ModelState.AddModelError("", $"Seçtiðiniz saatte eðitmen dolu. (Eðitmenin Dolu Olduðu Aralýk: {busyStart:HH:mm} - {busyEnd:HH:mm}).");
                    }
                    else
                    {
                        // Sorun yoksa kaydet
                        _context.Appointments.Add(appointment);
                        _context.SaveChanges();
                        return RedirectToAction("MyAppointments");
                    }
                }
            }

            // --- HATA VARSA SAYFAYI TEKRAR DOLDUR ---
            var services = _context.Services.ToList();
            ViewBag.Services = new SelectList(services, "Id", "Name");
            ViewBag.ServicesList = services;

            var trainers = _context.Trainers.Select(t => new { Id = t.Id, Name = t.Name, ServiceId = t.ServiceId }).ToList();
            ViewBag.TrainersList = trainers;
            ViewBag.Trainers = new SelectList(_context.Trainers, "Id", "Name");

            return View(appointment);
        }

        // --- 3. KULLANICININ KENDÝ RANDEVULARI ---
        [HttpGet]
        public IActionResult MyAppointments()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null) return RedirectToAction("Login", "Account");

            var appointments = _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .Where(a => a.UserId == user.Id)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();

            return View(appointments);
        }
    }
}