using FitnessCenter.WebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/trainers")]
public class TrainersApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public TrainersApiController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var trainers = _context.Trainers.AsNoTracking()
            .Select(t => new
            {
                id = t.Id,
                name = t.Name,
                speciality = t.Speciality,
                imageUrl = t.ImageUrl
            })
            .ToList();

        return Ok(trainers);
    }
}