using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenter.WebApp.Data;

namespace FitnessCenter.WebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentsApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/AppointmentsApi/user/{userId}
        // Belirli bir kullanıcının randevularını getir
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserAppointments(int userId)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .Where(a => a.UserId == userId)
                .Select(a => new
                {
                    a.Id,
                    AppointmentDate = a.AppointmentDate.ToString("yyyy-MM-dd HH:mm"),
                    TrainerName = a.Trainer!.Name,
                    ServiceName = a.Service!.Name,
                    a.Status,
                    a.IsApproved,
                    a.Notes
                })
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return Ok(appointments);
        }

        // GET: api/AppointmentsApi/trainer/{trainerId}
        // Eğitmenin randevularını getir
        [HttpGet("trainer/{trainerId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetTrainerAppointments(int trainerId)
        {
            var appointments = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .Where(a => a.TrainerId == trainerId)
                .Select(a => new
                {
                    a.Id,
                    AppointmentDate = a.AppointmentDate.ToString("yyyy-MM-dd HH:mm"),
                    UserName = a.User!.FullName,
                    UserEmail = a.User.Email,
                    ServiceName = a.Service!.Name,
                    a.Status,
                    a.IsApproved
                })
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return Ok(appointments);
        }

        // GET: api/AppointmentsApi/pending
        // Bekleyen randevuları getir
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<object>>> GetPendingAppointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .Where(a => a.Status == "Beklemede")
                .Select(a => new
                {
                    a.Id,
                    AppointmentDate = a.AppointmentDate.ToString("yyyy-MM-dd HH:mm"),
                    UserName = a.User!.FullName,
                    TrainerName = a.Trainer!.Name,
                    ServiceName = a.Service!.Name,
                    CreatedDate = a.CreatedDate.ToString("yyyy-MM-dd HH:mm")
                })
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();

            return Ok(appointments);
        }
    }
}