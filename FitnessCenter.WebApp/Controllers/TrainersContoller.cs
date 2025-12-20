using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenter.WebApp.Data;
using FitnessCenter.WebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace FitnessCenter.WebApp.Controllers
{
    public class TrainersController : Controller
    {
        private readonly AppDbContext _context;

        public TrainersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Trainers (Herkes görebilir)
        public async Task<IActionResult> Index()
        {
            var trainers = await _context.Trainers
                .Where(t => t.IsActive) // Sadece aktif eğitmenler
                .OrderBy(t => t.Name)
                .ToListAsync();

            return View(trainers);
        }

        // GET: Trainers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // GET: Trainers/Create (Sadece Admin)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trainers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Speciality,ImageUrl,Bio,IsActive")] Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainer);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Eğitmen başarıyla eklendi! ";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        // GET:  Trainers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }
            return View(trainer);
        }

        // POST: Trainers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Speciality,ImageUrl,Bio,IsActive")] Trainer trainer)
        {
            if (id != trainer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainer);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Eğitmen başarıyla güncellendi!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerExists(trainer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        // GET:  Trainers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // POST: Trainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Eğitmen başarıyla silindi!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TrainerExists(int id)
        {
            return _context.Trainers.Any(e => e.Id == id);
        }
    }
}