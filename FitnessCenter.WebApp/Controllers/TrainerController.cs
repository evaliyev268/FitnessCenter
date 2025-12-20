using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenter.WebApp.Data;
using System.Security.Claims;

namespace FitnessCenter.WebApp.Controllers
{
    // Sadece "Trainer" rolü girebilir
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Trainer")]
    public class TrainerController : Controller
    {
        private readonly AppDbContext _context;

        public TrainerController(AppDbContext context)
        {
            _context = context;
        }

        // Eðitmenin Panel Ana Sayfasý
        public IActionResult Index()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var trainerProfile = _context.Trainers.FirstOrDefault(t => t.Email == userEmail);

            if (trainerProfile == null) return View("Error");

            var appointments = _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .Where(a => a.TrainerId == trainerProfile.Id)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();

            return View(appointments);
        }

        // --- ONAYLA (Süre Kontrollü) ---
        [HttpPost]
        public IActionResult Approve(int id)
        {
            var appointment = _context.Appointments.Include(a => a.Service).FirstOrDefault(a => a.Id == id);

            if (appointment != null)
            {
                // Onaylanacak randevunun süresi
                DateTime newStart = appointment.AppointmentDate;
                DateTime newEnd = newStart.AddMinutes(appointment.Service.Duration);

                // Veritabanýndaki diðer "Onaylý" randevularla çakýþýyor mu?
                var conflictingAppt = _context.Appointments
                    .Include(a => a.Service)
                    .Where(a => a.TrainerId == appointment.TrainerId && a.Status == "Onaylandý" && a.Id != id)
                    .ToList()
                    .Any(existing =>
                        newStart < existing.AppointmentDate.AddMinutes(existing.Service.Duration) &&
                        existing.AppointmentDate < newEnd
                    );

                if (conflictingAppt)
                {
                    TempData["Error"] = "Bu saat aralýðýnda çakýþan baþka bir ONAYLI randevu var! Onaylayamazsýnýz.";
                }
                else
                {
                    appointment.Status = "Onaylandý";

                    // ÇAKIÞAN 'BEKLEYEN' TALEPLERÝ OTOMATÝK REDDET
                    // Çünkü artýk o saat doldu, diðerleri beklememeli.
                    var otherRequests = _context.Appointments
                       .Include(a => a.Service)
                       .Where(a => a.TrainerId == appointment.TrainerId && a.Status == "Bekliyor" && a.Id != id)
                       .ToList();

                    foreach (var req in otherRequests)
                    {
                        DateTime reqStart = req.AppointmentDate;
                        DateTime reqEnd = reqStart.AddMinutes(req.Service.Duration);

                        // Çakýþma varsa reddet
                        if (newStart < reqEnd && reqStart < newEnd)
                        {
                            req.Status = "Reddedildi (Dolu)";
                        }
                    }

                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        // --- REDDET ---
        [HttpPost]
        public IActionResult Reject(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment != null)
            {
                appointment.Status = "Reddedildi";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}