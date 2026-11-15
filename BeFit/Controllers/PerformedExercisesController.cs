using System.Security.Claims;
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
    [Authorize]
    public class PerformedExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PerformedExercisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        private void PopulateDropDowns(string userId, object? selectedExerciseType = null, object? selectedSession = null)
        {
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", selectedExerciseType);

            var userSessions = _context.TrainingSessions
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.StartTime)
                .Select(s => new
                {
                    s.Id,
                    Label = $"{s.StartTime:yyyy-MM-dd HH:mm} - {s.EndTime:HH:mm}"
                })
                .ToList();

            ViewData["TrainingSessionId"] = new SelectList(userSessions, "Id", "Label", selectedSession);
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var exercises = await _context.PerformedExercises
                .Include(p => p.ExerciseType)
                .Include(p => p.TrainingSession)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.TrainingSession!.StartTime)
                .ToListAsync();
            return View(exercises);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userId = GetUserId();
            var exercise = await _context.PerformedExercises
                .Include(p => p.ExerciseType)
                .Include(p => p.TrainingSession)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (exercise == null) return NotFound();
            return View(exercise);
        }

        public IActionResult Create()
        {
            PopulateDropDowns(GetUserId());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ExerciseTypeId,TrainingSessionId,Weight,Sets,Repetitions")] PerformedExercise performedExercise)
        {
            var userId = GetUserId();

            var sessionExists = await _context.TrainingSessions
                .AnyAsync(s => s.Id == performedExercise.TrainingSessionId && s.UserId == userId);
            if (!sessionExists)
            {
                ModelState.AddModelError("TrainingSessionId", "Wybrana sesja treningowa nie należy do Ciebie.");
            }

            ModelState.Remove(nameof(PerformedExercise.UserId));
            performedExercise.UserId = userId;

            if (ModelState.IsValid)
            {
                _context.Add(performedExercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDowns(userId, performedExercise.ExerciseTypeId, performedExercise.TrainingSessionId);
            return View(performedExercise);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = GetUserId();
            var exercise = await _context.PerformedExercises
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (exercise == null) return NotFound();

            PopulateDropDowns(userId, exercise.ExerciseTypeId, exercise.TrainingSessionId);
            return View(exercise);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ExerciseTypeId,TrainingSessionId,Weight,Sets,Repetitions")] PerformedExercise performedExercise)
        {
            if (id != performedExercise.Id) return NotFound();

            var userId = GetUserId();
            var existingExercise = await _context.PerformedExercises
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (existingExercise == null) return NotFound();

            var sessionExists = await _context.TrainingSessions
                .AnyAsync(s => s.Id == performedExercise.TrainingSessionId && s.UserId == userId);
            if (!sessionExists)
            {
                ModelState.AddModelError("TrainingSessionId", "Wybrana sesja treningowa nie należy do Ciebie.");
            }

            ModelState.Remove(nameof(PerformedExercise.UserId));
            performedExercise.UserId = userId;

            if (ModelState.IsValid)
            {
                _context.Update(performedExercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDowns(userId, performedExercise.ExerciseTypeId, performedExercise.TrainingSessionId);
            return View(performedExercise);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = GetUserId();
            var exercise = await _context.PerformedExercises
                .Include(p => p.ExerciseType)
                .Include(p => p.TrainingSession)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (exercise == null) return NotFound();
            return View(exercise);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetUserId();
            var exercise = await _context.PerformedExercises
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (exercise != null)
            {
                _context.PerformedExercises.Remove(exercise);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}