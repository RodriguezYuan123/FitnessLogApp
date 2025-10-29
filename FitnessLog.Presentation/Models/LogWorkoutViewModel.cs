using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessLog.Presentation.Models // Or Your.Project.ViewModels
{
    public class LogWorkoutViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Exercise Name")] // Improves label generation
        public string ExerciseName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Reps must be zero or positive.")]
        public int Reps { get; set; } // Use 0 for duration-based

        [Range(0.0, double.MaxValue, ErrorMessage = "Duration must be zero or positive.")]
        [Display(Name = "Duration (Minutes)")]
        public double DurationInMinutes { get; set; } // Use 0 for rep-based

        [Required]
        [DataType(DataType.Date)] // Helps with date input rendering
        public DateTime Date { get; set; } = DateTime.Today; // Default to today
    }
}