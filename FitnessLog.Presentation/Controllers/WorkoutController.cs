using Microsoft.AspNetCore.Mvc;
using FitnessLog.Service;
using FitnessLog.Domain;
using FitnessLog.Presentation.Models; // <-- Add this using for the ViewModel
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FitnessLog.Presentation.Controllers
{
    [Authorize]
    public class WorkoutController : Controller
    {
        private readonly IWorkoutService _workoutService;
        private readonly UserManager<ApplicationUser> _userManager;

        public WorkoutController(IWorkoutService workoutService, UserManager<ApplicationUser> userManager)
        {
            _workoutService = workoutService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) { return Challenge(); }
            var workouts = await _workoutService.GetWorkoutsByUserIdAsync(userId);
            return View(workouts);
        }

        // GET: /Workout/LogWorkout
        // Now returns the ViewModel to the view
        public IActionResult LogWorkout()
        {
            // Pass a new ViewModel with the Date defaulted to today
            var viewModel = new LogWorkoutViewModel
            {
                Date = DateTime.Today
            };
            return View(viewModel);
        }

        // POST: /Workout/LogWorkout
        // Now accepts the ViewModel from the form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogWorkout(LogWorkoutViewModel viewModel) // <-- Accept ViewModel
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }

            // Check if the ViewModel is valid (based on ViewModel's rules)
            // ApplicationUserId is NOT checked here anymore!
            if (ModelState.IsValid)
            {
                // **Map** the valid ViewModel to your Domain Model
                var workout = new Workout
                {
                    ExerciseName = viewModel.ExerciseName,
                    Reps = viewModel.Reps,
                    DurationInMinutes = viewModel.DurationInMinutes,
                    Date = viewModel.Date,
                    ApplicationUserId = userId // <-- Assign the User ID *here*
                };

                await _workoutService.LogWorkoutAsync(workout);
                return RedirectToAction(nameof(Index));
            }

            // If ViewModel is invalid, return the view with the entered data and validation errors
            return View(viewModel); // <-- Return the ViewModel back to the view
        }
    }
}