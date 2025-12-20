using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering; // SelectList için gerekli
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

        // --- EÐÝTMEN ÝÞLEMLERÝ ---
        [HttpGet]
        public IActionResult CreateTrainer()
        {
            // DÜZELTME BURADA YAPILDI: Hizmet listesini gönderiyoruz
            ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult CreateTrainer(Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                _context.Trainers.Add(trainer);
                _context.SaveChanges();
                return RedirectToAction("Trainers");
            }
            // Hata olursa listeyi tekrar doldur
            ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
            return View(trainer);
        }

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

        // --- KULLANICI SÝLME ---
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
    }
}