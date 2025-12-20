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

        // --- RANDEVU ALMA SAYFASI (GET) ---
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Services = new SelectList(_context.Services, "Id", "Name");

            // DÜZELTME: Eðitmenleri ServiceId ile beraber gönderiyoruz ki JS filtreleyebilsin
            var trainers = _context.Trainers.Select(t => new {
                Id = t.Id,
                Name = t.Name,
                ServiceId = t.ServiceId
            }).ToList();

            ViewBag.TrainersList = trainers;

            return View();
        }

        // --- RANDEVUYU KAYDETME (POST) ---
        [HttpPost]
        public IActionResult Create(Appointment appointment)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user != null)
            {
                appointment.UserId = user.Id;

                if (appointment.AppointmentDate < DateTime.Now)
                {
                    ModelState.AddModelError("", "Geçmiþ bir tarihe randevu alamazsýnýz.");
                }
                else
                {
                    _context.Appointments.Add(appointment);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Trainers = new SelectList(_context.Trainers, "Id", "Name");
            ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
            return View(appointment);
        }
    }
}