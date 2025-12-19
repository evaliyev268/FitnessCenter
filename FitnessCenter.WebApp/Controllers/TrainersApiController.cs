using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenter.WebApp.Data;
using System.Linq;

namespace FitnessCenter.WebApp.Controllers
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

        [HttpGet]
        public IActionResult GetTrainers()
        {
            var trainers = _context.Trainers
                .Include(t => t.Service)
                .Select(t => new
                {
                    Id = t.Id,
                    AdSoyad = t.Name,
                    Uzmanlik = t.Speciality,
                    VerdigiHizmet = t.Service.Name,
                    Fiyat = t.Service.Price
                })
                .ToList();

            return Ok(trainers);
        }

        [HttpGet("search")]
        public IActionResult SearchTrainers(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                return BadRequest("Lütfen bir hizmet adý girin.");
            }

            var trainers = _context.Trainers
                .Include(t => t.Service)
                .Where(t => t.Service.Name.Contains(serviceName)) 
                .Select(t => new
                {
                    Id = t.Id,
                    AdSoyad = t.Name,
                    Branþ = t.Service.Name
                })
                .ToList();

            return Ok(trainers);
        }
    }
}