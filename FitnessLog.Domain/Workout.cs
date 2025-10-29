using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessLog.Domain
{
    public class Workout
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ExerciseName { get; set; } // e.g., "Squats", "Running"

        public int Reps { get; set; } // Number of repetitions (use 0 for duration-based)

        public double DurationInMinutes { get; set; } // Duration in minutes (use 0 for rep-based)

        [Required]
        public DateTime Date { get; set; } = DateTime.Today; // Default to today

        // Foreign Key to the Identity User
        [Required]
        public string ApplicationUserId { get; set; }
        // public ApplicationUser ApplicationUser { get; set; } // Navigation property (Defined later)
    }
}