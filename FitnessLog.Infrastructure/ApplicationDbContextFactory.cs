using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FitnessLog.Infrastructure
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            
            // When EF tools run, the current directory is the startup project directory
            var presentationPath = basePath;
            
            // Check if appsettings.json exists in current directory
            if (!File.Exists(Path.Combine(presentationPath, "appsettings.json")))
            {
                // If not, try from solution root
                presentationPath = Path.Combine(basePath, "FitnessLog.Presentation");
            }
            
            if (!File.Exists(Path.Combine(presentationPath, "appsettings.json")))
            {
                throw new FileNotFoundException(
                    $"Could not find appsettings.json. Base path: {basePath}");
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(presentationPath)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                // Corrected variable name used here
                throw new InvalidOperationException("Could not find 'DefaultConnection' in appsettings.json from " + presentationPath);
            }

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(connectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }
}