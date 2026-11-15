using System.Security.Claims;
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var fourWeeksAgo = DateTime.Now.AddDays(-28);

            var stats = await _context.PerformedExercises
                .Include(p => p.ExerciseType)
                .Include(p => p.TrainingSession)
                .Where(p => p.UserId == userId && p.TrainingSession != null && p.TrainingSession.StartTime >= fourWeeksAgo)
                .GroupBy(p => p.ExerciseType!.Name)
                .Select(g => new ExerciseStatisticViewModel
                {
                    ExerciseName = g.Key,
                    ExecutionCount = g.Count(),
                    TotalRepetitions = g.Sum(x => x.Sets * x.Repetitions),
                    AverageWeight = Math.Round(g.Average(x => x.Weight), 2),
                    MaxWeight = g.Max(x => x.Weight)
                })
                .OrderByDescending(s => s.ExecutionCount)
                .ToListAsync();

            return View(stats);
        }
    }
}