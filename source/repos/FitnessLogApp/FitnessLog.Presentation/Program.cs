using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FitnessLog.Domain;       // For ApplicationUser
using FitnessLog.Infrastructure; // For ApplicationDbContext
using FitnessLog.Service;      // To register your service layer (Step 7)

var builder = WebApplication.CreateBuilder(args);

// ====================================================================
// SERVICES CONFIGURATION (Dependency Injection)
// ====================================================================

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 1. Configure EF Core DbContext (SQL Server implementation)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 1b. REQUIRED: Setup for In-Memory Option (Use one or the other based on config/testing)
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseInMemoryDatabase("FitnessLogInMemoryDb"));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 2. Configure Identity Service with Complex Password Policy
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // *** REQUIRED PASSWORD POLICY: at least 2 uppercase, 3 numbers, 3 symbols ***

    // Enforce existence of required character types
    options.Password.RequireDigit = true;         // Ensures >= 1 Number (Required 3)
    options.Password.RequireNonAlphanumeric = true; // Ensures >= 1 Symbol (Required 3)
    options.Password.RequireUppercase = true;     // Ensures >= 1 Uppercase (Required 2)
    options.Password.RequireLowercase = true;     // Standard practice

    // Enforce high complexity (Minimum length & unique chars)
    options.Password.RequiredLength = 12;         // Use a strong length to encourage complexity
    options.Password.RequiredUniqueChars = 6;     // Encourage varied characters

    // Other settings (e.g., Lockout settings can be configured here too)
    // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    // options.Lockout.MaxFailedAccessAttempts = 5;

})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

// 3. Register Application Services (Placeholder for Step 7)
builder.Services.AddScoped<IWorkoutService, WorkoutService>(); // Add this line!

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// ====================================================================
// MIDDLEWARE PIPELINE CONFIGURATION
// ====================================================================

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Must come before UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Maps the scaffolded Identity Razor Pages
app.MapRazorPages();

app.Run();