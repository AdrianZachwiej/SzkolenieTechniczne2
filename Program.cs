using Microsoft.EntityFrameworkCore;
using ProjektAdrianZachwiej56233.Data;
using ProjektAdrianZachwiej56233.Services;
using ProjektAdrianZachwiej56233.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ReservationService>();

var app = builder.Build();

// Ensure database is created and initial data is seeded (development only)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Ensure database is created
    await dbContext.Database.EnsureCreatedAsync();

    // Seed initial data if no tables exist
    if (!dbContext.Tables.Any())
    {
        dbContext.Tables.AddRange(
            new Table { Number = 1, Seats = 4, IsAvailable = true },
            new Table { Number = 2, Seats = 2, IsAvailable = true },
            new Table { Number = 3, Seats = 6, IsAvailable = true },
            new Table { Number = 4, Seats = 4, IsAvailable = true },
            new Table { Number = 5, Seats = 2, IsAvailable = true }
        );

        await dbContext.SaveChangesAsync();
    }
}

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Show detailed errors in development
}
else
{
    // Global exception handling for production
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Map controllers (API and MVC routes)
app.MapControllers();

// Fallback to static file (for SPA)
app.MapFallbackToFile("index.html");

// Run the application
app.Run();
