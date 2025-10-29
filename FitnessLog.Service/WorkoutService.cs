using FitnessLog.Domain;
using FitnessLog.Infrastructure; // Needed for ApplicationDbContext
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessLog.Service
{
    public class WorkoutService : IWorkoutService
    {
        private readonly ApplicationDbContext _context;

        // Constructor Injection: DbContext is provided by Dependency Injection
        public WorkoutService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogWorkoutAsync(Workout workout)
        {
            if (workout == null)
            {
                throw new ArgumentNullException(nameof(workout));
            }

            // Ensure the date is set (can add more validation)
            if (workout.Date == default)
            {
                workout.Date = DateTime.Today;
            }

            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Workout>> GetWorkoutsByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            return await _context.Workouts
                                 .Where(w => w.ApplicationUserId == userId)
                                 .OrderByDescending(w => w.Date) // Show newest first
                                 .ToListAsync();
        }

        // TODO: Implement ExportWorkoutsToCsvAsync in a later step
        // public Task<byte[]> ExportWorkoutsToCsvAsync(string userId) 
        // {
        //     throw new NotImplementedException(); 
        // }
    }
}