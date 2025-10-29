using FitnessLog.Domain; // Needed for the Workout entity
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessLog.Service
{
    public interface IWorkoutService
    {
        // Log a new workout for a specific user
        Task LogWorkoutAsync(Workout workout);

        // Get all workouts for a specific user
        Task<List<Workout>> GetWorkoutsByUserIdAsync(string userId);

        // TODO: Export workouts to CSV (Implementation in a later step)
        // Task<byte[]> ExportWorkoutsToCsvAsync(string userId); 
    }
}