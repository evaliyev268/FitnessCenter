using FitnessCenter.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FitnessCenter.Controllers
{
    public class TrainersApiReq : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<Trainer> trainers = new();

            HttpClient h = new HttpClient();
            var resp = await h.GetAsync("https://localhost:7112/api/trainers");

            var respStr = await resp.Content.ReadAsStringAsync();
            trainers = JsonConvert.DeserializeObject<List<Trainer>>(respStr);

            return View(trainers);
        }
    }
}