using Microsoft.EntityFrameworkCore;
using CourseCompass.API.Data;
using CourseCompass.API.Services;

// Create a WebApplication builder - this sets up your web server
var builder = WebApplication.CreateBuilder(args);

// ===== ADDING SERVICES TO THE CONTAINER =====
// Services are like "tools" your application can use

// Add Controllers: This tells the app we're using MVC controllers
builder.Services.AddControllers();

// Add API Explorer: This helps generate API documentation
builder.Services.AddEndpointsApiExplorer();

// Add Swagger: This creates the nice UI you see at /swagger
builder.Services.AddSwaggerGen();

// Add Entity Framework: This connects your DbContext to the database
builder.Services.AddDbContext<CourseCompassDbContext>(options =>
    // Get connection string from appsettings.json and use SQL Server
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add our custom services
builder.Services.AddScoped<SeedDataService>();

// Add CORS: Cross-Origin Resource Sharing - allows Angular app to call your API
// Without this, your Angular app would get "blocked by CORS policy" errors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            // Allow requests from Angular dev server (localhost:4200)
            policy.WithOrigins("http://localhost:4200")
                  // Allow any headers (like Content-Type, Authorization)
                  .AllowAnyHeader()
                  // Allow any HTTP methods (GET, POST, PUT, DELETE)
                  .AllowAnyMethod();
        });
});

// Build the application with all the services we just configured
var app = builder.Build();

// ===== SEED DATABASE WITH SAMPLE DATA =====
// This runs once when the application starts
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CourseCompassDbContext>();
        var seedService = services.GetRequiredService<SeedDataService>();

        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed sample data
        await seedService.SeedDataAsync();
    }
    catch (Exception ex)
    {
        // Log any errors that occur during seeding
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// ===== CONFIGURE THE HTTP REQUEST PIPELINE =====
// This is the "order of operations" for handling incoming requests

// If we're in Development mode, enable Swagger UI
if (app.Environment.IsDevelopment())
{
    // Enable Swagger JSON endpoint
    app.UseSwagger();
    // Enable Swagger UI at /swagger
    app.UseSwaggerUI();
}

// Redirect HTTP requests to HTTPS (security)
app.UseHttpsRedirection();

// Enable CORS (must come before Authorization)
app.UseCors("AllowAngular");

// Enable Authorization middleware (we'll use this later for Firebase auth)
app.UseAuthorization();

// Map controller routes (so /api/courses goes to CoursesController)
app.MapControllers();

// Start the web server and listen for requests
app.Run();