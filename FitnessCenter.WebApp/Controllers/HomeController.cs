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
            var trainers = _context.Trainers.ToList();
            return View(trainers);
        }

        public IActionResult Services()
        {
            var services = _context.Services.ToList();
            return View(services);
        }
    }
}