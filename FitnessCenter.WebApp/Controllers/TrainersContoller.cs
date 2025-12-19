using Microsoft.AspNetCore.Mvc;
using FitnessCenter.WebApp.Models;

namespace FitnessCenter.WebApp.Controllers
{
    public class TrainersController : Controller
    {
        public IActionResult Index()
        {
            // Veri tabanı olmadığı için sahte liste oluşturuyoruz
            var trainers = new List<Trainer>
            {
                new Trainer { Id = 1, Name = "Yusuf Ziya GÖk", Speciality = "Vücut Geliştirme", ImageUrl = "https://randomuser.me/api/portraits/men/32.jpg" },
                new Trainer { Id = 2, Name = "Şahin Bağcı", Speciality = "Pilates & Yoga", ImageUrl = "https://randomuser.me/api/portraits/men/59.jpg" },
                new Trainer { Id = 3, Name = "Okan Koca", Speciality = "Crossfit", ImageUrl = "https://randomuser.me/api/portraits/men/85.jpg" }
            };

            return View(trainers);
        }
    }
}