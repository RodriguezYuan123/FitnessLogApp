using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FitnessLog.Domain;       // For ApplicationUser
using FitnessLog.Infrastructure; // For ApplicationDbContext
using FitnessLog.Service;      // For IWorkoutService/WorkoutService

var builder = WebApplication.CreateBuilder(args);

// ====================================================================
// SERVICES CONFIGURATION (Dependency Injection)
// ====================================================================

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 1. Configure EF Core DbContext (SQL Server implementation)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 1b. REQUIRED: Setup for In-Memory Option (Uncomment to use)
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseInMemoryDatabase("FitnessLogInMemoryDb"));

// Add services required for pages like Database error page
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
    // options.SignIn.RequireConfirmedAccount = true;
    // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    // options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

// 3. Register Application Services
builder.Services.AddScoped<IWorkoutService, WorkoutService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Required for Identity UI pages

var app = builder.Build();

// ====================================================================
// MIDDLEWARE PIPELINE CONFIGURATION
// ====================================================================

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint(); // Provides helpful error page for EF migrations
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// ****** CORRECT ORDER IS IMPORTANT ******
app.UseRouting(); // 1st: Determines which endpoint to use

app.UseAuthentication(); // 2nd: Identifies the user (checks cookie)
app.UseAuthorization(); // 3rd: Verifies if the user is allowed access

// ****** END CORRECT ORDER ******

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Maps the scaffolded Identity Razor Pages (like Login, Register, Manage)
app.MapRazorPages();

app.Run();