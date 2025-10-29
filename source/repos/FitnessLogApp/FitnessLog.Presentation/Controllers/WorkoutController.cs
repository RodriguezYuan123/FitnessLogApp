using Microsoft.AspNetCore.Mvc;
using FitnessLog.Service;        // For IWorkoutService
using FitnessLog.Domain;        // For Workout and ApplicationUser
using Microsoft.AspNetCore.Identity; // For UserManager
using Microsoft.AspNetCore.Authorization; // To protect controller actions
using System.Security.Claims;     // To get current user ID
using System.Threading.Tasks;

namespace FitnessLog.Presentation.Controllers
{
    [Authorize] // Ensures only logged-in users can access these actions
    public class WorkoutController : Controller
    {
        private readonly IWorkoutService _workoutService;
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor Injection
        public WorkoutController(IWorkoutService workoutService, UserManager<ApplicationUser> userManager)
        {
            _workoutService = workoutService;
            _userManager = userManager;
        }

        // GET: /Workout or /Workout/Index
        // Displays the user's workout history
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get current logged-in user's ID
            if (userId == null)
            {
                // Should not happen if [Authorize] is working, but good practice
                return Challenge(); // Redirects to login
            }

            var workouts = await _workoutService.GetWorkoutsByUserIdAsync(userId);
            return View(workouts); // Pass the list of workouts to the View
        }

        // GET: /Workout/LogWorkout
        // Displays the form to log a new workout
        public IActionResult LogWorkout()
        {
            return View(); // Just shows the empty form
        }

        // POST: /Workout/LogWorkout
        // Handles the submission of the new workout form
        [HttpPost]
        [ValidateAntiForgeryToken] // Protects against CSRF attacks
        public async Task<IActionResult> LogWorkout(Workout workout) // Model binding automatically creates Workout object from form data
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }

            // Important: Assign the current user's ID to the workout record
            workout.ApplicationUserId = userId;

            // Optional: Server-side validation
            if (ModelState.IsValid)
            {
                await _workoutService.LogWorkoutAsync(workout);
                // Redirect back to the history page after successful logging
                return RedirectToAction(nameof(Index));
            }

            // If model state is invalid, return the view with the entered data and validation errors
            return View(workout);
        }
    }
}