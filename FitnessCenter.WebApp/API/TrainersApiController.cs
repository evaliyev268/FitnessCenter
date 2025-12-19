using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenter.WebApp.Data;
using FitnessCenter.WebApp.Models;

namespace FitnessCenter.WebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainersApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrainersApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TrainersApi
        // Tüm aktif eğitmenleri getir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetTrainers()
        {
            var trainers = await _context.Trainers
                .Where(t => t.IsActive)
                .Select(t => new
                {
                    t.Id,
                    t.Name,
                    t.Speciality,
                    t.Email,
                    t.HourlyRate,
                    WorkStartTime = t.WorkStartTime.HasValue ? t.WorkStartTime.Value.ToString(@"hh\:mm") : null,
                    WorkEndTime = t.WorkEndTime.HasValue ? t.WorkEndTime.Value.ToString(@"hh\:mm") : null,
                    t.WorkingDays
                })
                .OrderBy(t => t.Name)
                .ToListAsync();

            return Ok(trainers);
        }

        // GET: api/TrainersApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trainer>> GetTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer == null)
            {
                return NotFound(new { message = "Eğitmen bulunamadı" });
            }

            return Ok(trainer);
        }

        // GET: api/TrainersApi/available? date=2025-12-21&time=10:00
        // Belirli tarih ve saatte uygun eğitmenleri getir
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<object>>> GetAvailableTrainers([FromQuery] DateTime date)
        {
            var requestedTime = date.TimeOfDay;

            // O tarih ve saatte randevusu olmayan eğitmenler
            var bookedTrainerIds = await _context.Appointments
                .Where(a => a.AppointmentDate == date && a.Status != "Reddedildi")
                .Select(a => a.TrainerId)
                .ToListAsync();

            var availableTrainers = await _context.Trainers
                .Where(t => t.IsActive
                         && !bookedTrainerIds.Contains(t.Id)
                         && t.WorkStartTime <= requestedTime
                         && t.WorkEndTime >= requestedTime)
                .Select(t => new
                {
                    t.Id,
                    t.Name,
                    t.Speciality,
                    t.HourlyRate,
                    WorkTime = $"{t.WorkStartTime:hh\\:mm} - {t.WorkEndTime:hh\\:mm}"
                })
                .ToListAsync();

            return Ok(availableTrainers);
        }

        // GET: api/TrainersApi/byspeciality/Yoga
        // Uzmanlık alanına göre filtreleme
        [HttpGet("byspeciality/{speciality}")]
        public async Task<ActionResult<IEnumerable<object>>> GetTrainersBySpeciality(string speciality)
        {
            var trainers = await _context.Trainers
                .Where(t => t.IsActive && t.Speciality.Contains(speciality))
                .Select(t => new
                {
                    t.Id,
                    t.Name,
                    t.Speciality,
                    t.HourlyRate
                })
                .ToListAsync();

            if (!trainers.Any())
            {
                return NotFound(new { message = $"'{speciality}' uzmanlık alanında eğitmen bulunamadı" });
            }

            return Ok(trainers);
        }
    }
}