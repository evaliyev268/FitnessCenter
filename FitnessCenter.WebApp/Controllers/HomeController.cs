using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenter.Models;
using FitnessCenter.WebApp.Data;

namespace FitnessCenter.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Ana sayfada gösterilecek veriler
        ViewBag.Trainers = await _context.Trainers
            .Where(t => t.IsActive)
            .Take(3) // İlk 3 antrenör
            .ToListAsync();

        ViewBag.Services = await _context.Services
            .Where(s => s.IsActive)
            .Take(6) // İlk 6 hizmet
            .ToListAsync();

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}