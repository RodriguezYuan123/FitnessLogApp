using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FitnessLog.Domain
{
    // Inherits from IdentityUser to add custom properties if needed later.
    // We are using the default IdentityUser for now.
    public class ApplicationUser : IdentityUser
    {
        // Add any custom user properties here, e.g.,
        // public string FullName { get; set; }

        // Navigation property for the user's workouts
        public ICollection<Workout> Workouts { get; set; }
    }
}