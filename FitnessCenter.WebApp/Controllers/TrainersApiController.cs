using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenter.WebApp.Data;
using System.Linq;

namespace FitnessCenter.WebApp.Controllers // DÝKKAT: .Api kýsmýný sildik
{
    // Bu satýr sayesinde adres yine "api/trainersapi" þeklinde çalýþýr
    [Route("api/[controller]")]
    [ApiController]
    public class TrainersApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrainersApiController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Tüm Eðitmenleri Getiren API (GET: api/trainersapi)
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

        // 2. Filtreleme Yapan API (GET: api/trainersapi/search?serviceName=Yoga)
        [HttpGet("search")]
        public IActionResult SearchTrainers(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                return BadRequest("Lütfen bir hizmet adý girin.");
            }

            var trainers = _context.Trainers
                .Include(t => t.Service)
                .Where(t => t.Service.Name.Contains(serviceName)) // LINQ ile filtreleme
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