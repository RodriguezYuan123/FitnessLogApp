using FitnessLog.Domain; // Needed to reference the Workout entity
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessLog.Infrastructure
{
    // IdentityDbContext<TUser> is used to include default Identity tables
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet for your core application data
        public DbSet<Workout> Workouts { get; set; }

        // Optional: Custom configurations or fluent API can go here
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}