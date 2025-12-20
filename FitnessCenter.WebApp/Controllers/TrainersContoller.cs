using Microsoft.AspNetCore.Mvc;
using FitnessCenter.WebApp.Data;
using System.Linq;

namespace FitnessCenter.WebApp.Controllers
{
    public class TrainersController : Controller
    {
        private readonly AppDbContext _context;

        public TrainersController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // SAHTE LİSTEYİ SİLDİK. Gerçek veritabanından çekiyoruz.
            var trainers = _context.Trainers.ToList();
            return View(trainers);
        }
    }
}