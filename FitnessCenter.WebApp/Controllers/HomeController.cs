using Microsoft.AspNetCore.Mvc;
using FitnessCenter.WebApp.Data;
using System.Linq;

namespace FitnessCenter.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Ana sayfada 'Uzman Kadromuz' kýsmýný göstermek için eðitmenleri gönderiyoruz
            var trainers = _context.Trainers.ToList();
            return View(trainers);
        }

        // Privacy SÝLÝNDÝ, yerine Services GELDÝ
        public IActionResult Services()
        {
            var services = _context.Services.ToList();
            return View(services);
        }
    }
}