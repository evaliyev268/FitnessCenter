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

        [HttpGet]
        public IActionResult Create()
        {
            var services = _context.Services.ToList();

            ViewBag.Services = new SelectList(services, "Id", "Name");

            ViewBag.ServicesList = services;

            var trainers = _context.Trainers.Select(t => new {
                Id = t.Id,
                Name = t.Name,
                ServiceId = t.ServiceId
            }).ToList();

            ViewBag.TrainersList = trainers;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Appointment appointment)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            var selectedService = _context.Services.Find(appointment.ServiceId);

            if (user != null && selectedService != null)
            {
                appointment.UserId = user.Id;
                appointment.Status = "Bekliyor";

                if (appointment.AppointmentDate < DateTime.Now)
                {
                    ModelState.AddModelError("", "Geçmiþ bir tarihe randevu alamazsýnýz.");
                }
                else
                {

                    DateTime newStart = appointment.AppointmentDate;
                    DateTime newEnd = newStart.AddMinutes(selectedService.Duration);

       
                    var existingAppointments = _context.Appointments
                        .Include(a => a.Service)
                        .Where(a => a.TrainerId == appointment.TrainerId && a.Status == "Onaylandý")
                        .ToList();

                    Appointment? conflictingAppointment = null;

                    foreach (var existing in existingAppointments)
                    {
                        DateTime existingStart = existing.AppointmentDate;
                        DateTime existingEnd = existingStart.AddMinutes(existing.Service.Duration);

                        if (newStart < existingEnd && existingStart < newEnd)
                        {
                            conflictingAppointment = existing; 
                            break; 
                        }
                    }

                    if (conflictingAppointment != null)
                    {
                        DateTime busyStart = conflictingAppointment.AppointmentDate;
                        DateTime busyEnd = busyStart.AddMinutes(conflictingAppointment.Service.Duration);

                        ModelState.AddModelError("", $"Seçtiðiniz saatte eðitmen dolu. (Eðitmenin Dolu Olduðu Aralýk: {busyStart:HH:mm} - {busyEnd:HH:mm}).");
                    }
                    else
                    {
                        _context.Appointments.Add(appointment);
                        _context.SaveChanges();
                        return RedirectToAction("MyAppointments");
                    }
                }
            }

            var services = _context.Services.ToList();
            ViewBag.Services = new SelectList(services, "Id", "Name");
            ViewBag.ServicesList = services;

            var trainers = _context.Trainers.Select(t => new { Id = t.Id, Name = t.Name, ServiceId = t.ServiceId }).ToList();
            ViewBag.TrainersList = trainers;
            ViewBag.Trainers = new SelectList(_context.Trainers, "Id", "Name");

            return View(appointment);
        }

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