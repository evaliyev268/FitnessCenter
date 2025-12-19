using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FitnessCenter.WebApp.Data;
using FitnessCenter.WebApp.Models;
using System.Security.Claims;

namespace FitnessCenter.WebApp.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        // ============================================
        // MEMBER:  RANDEVU ALMA SAYFASI
        // ============================================
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Trainers = await _context.Trainers
                .Where(t => t.IsActive)
                .OrderBy(t => t.Name)
                .ToListAsync();

            ViewBag.Services = await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            // Kullanıcı ID'sini al
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı!";
                return RedirectToAction("Login", "Account");
            }

            appointment.UserId = user.Id;
            appointment.CreatedDate = DateTime.Now;
            appointment.Status = "Beklemede";
            appointment.IsApproved = false;

            // ÇAKIŞMA KONTROLÜ - Aynı tarih/saat/antrenörde başka randevu var mı?
            var conflict = await _context.Appointments
                .AnyAsync(a => a.TrainerId == appointment.TrainerId
                            && a.AppointmentDate == appointment.AppointmentDate
                            && a.Status != "Reddedildi");

            if (conflict)
            {
                TempData["Error"] = "Seçtiğiniz tarih ve saatte bu eğitmenin başka bir randevusu var!  Lütfen farklı bir saat seçiniz.";

                ViewBag.Trainers = await _context.Trainers.Where(t => t.IsActive).ToListAsync();
                ViewBag.Services = await _context.Services.Where(s => s.IsActive).ToListAsync();
                return View(appointment);
            }

            // ANTRENÖR ÇALİŞMA SAATİ KONTROLÜ
            var trainer = await _context.Trainers.FindAsync(appointment.TrainerId);
            if (trainer != null && trainer.WorkStartTime.HasValue && trainer.WorkEndTime.HasValue)
            {
                var appointmentTime = appointment.AppointmentDate.TimeOfDay;
                if (appointmentTime < trainer.WorkStartTime.Value || appointmentTime > trainer.WorkEndTime.Value)
                {
                    TempData["Error"] = $"Seçilen saat eğitmenin çalışma saatleri dışında!  Çalışma saatleri: {trainer.WorkStartTime:hh\\:mm} - {trainer.WorkEndTime:hh\\:mm}";

                    ViewBag.Trainers = await _context.Trainers.Where(t => t.IsActive).ToListAsync();
                    ViewBag.Services = await _context.Services.Where(s => s.IsActive).ToListAsync();
                    return View(appointment);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Randevu talebiniz oluşturuldu!  Eğitmen onayladıktan sonra kesinleşecektir.";
                return RedirectToAction(nameof(MyAppointments));
            }

            ViewBag.Trainers = await _context.Trainers.Where(t => t.IsActive).ToListAsync();
            ViewBag.Services = await _context.Services.Where(s => s.IsActive).ToListAsync();
            return View(appointment);
        }

        // ============================================
        // MEMBER: RANDEVULARIM LİSTESİ
        // ============================================
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> MyAppointments()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı! ";
                return RedirectToAction("Login", "Account");
            }

            var appointments = await _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .Where(a => a.UserId == user.Id)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }

        // ============================================
        // TRAINER: GELEN RANDEVU TALEPLERİ
        // ============================================
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> TrainerAppointments()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            // Trainer'ın User email'i ile Trainer tablosundaki email'i eşleştir
            var trainer = await _context.Trainers
                .FirstOrDefaultAsync(t => t.Email == userEmail);

            if (trainer == null)
            {
                TempData["Error"] = "Eğitmen profili bulunamadı!";
                return RedirectToAction("Index", "Home");
            }

            var appointments = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .Where(a => a.TrainerId == trainer.Id)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }

        // ============================================
        // TRAINER: RANDEVU ONAYLAMA
        // ============================================
        [HttpPost]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> ApproveAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                TempData["Error"] = "Randevu bulunamadı!";
                return RedirectToAction(nameof(TrainerAppointments));
            }

            appointment.IsApproved = true;
            appointment.Status = "Onaylandı";
            await _context.SaveChangesAsync();

            TempData["Success"] = "Randevu başarıyla onaylandı! ";
            return RedirectToAction(nameof(TrainerAppointments));
        }

        // ============================================
        // TRAINER: RANDEVU REDDETME
        // ============================================
        [HttpPost]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> RejectAppointment(int id, string reason)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                TempData["Error"] = "Randevu bulunamadı!";
                return RedirectToAction(nameof(TrainerAppointments));
            }

            appointment.IsApproved = false;
            appointment.Status = "Reddedildi";
            appointment.Notes = reason ?? "Uygun değil";
            await _context.SaveChangesAsync();

            TempData["Success"] = "Randevu reddedildi. ";
            return RedirectToAction(nameof(TrainerAppointments));
        }

        // ============================================
        // ADMIN: TÜM RANDEVULAR
        // ============================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();

            return View(appointments);
        }

        // ============================================
        // ADMIN:  RANDEVU SİLME
        // ============================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Randevu başarıyla silindi! ";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}