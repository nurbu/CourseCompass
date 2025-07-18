using Microsoft.EntityFrameworkCore;
using CourseCompass.API.Models;

namespace CourseCompass.API.Data
{
    // DbContext is the main class that coordinates Entity Framework functionality
    // Think of it as your "database manager" - it handles all database operations
    public class CourseCompassDbContext : DbContext
    {
        // Constructor: This runs when the DbContext is created
        // The 'options' parameter contains database connection info
        public CourseCompassDbContext(DbContextOptions<CourseCompassDbContext> options) : base(options)
        {
            // We pass the options to the base DbContext class
            // This tells EF Core which database to connect to
        }

        // DbSet represents a table in your database
        // Students DbSet = Students table in database
        // Each Student object = one row in the Students table
        public DbSet<Student> Students { get; set; }

        // Courses DbSet = Courses table in database  
        // Each Course object = one row in the Courses table
        public DbSet<Course> Courses { get; set; }

        // OnModelCreating: This is where we define HOW our database tables should be structured
        // Think of it as the "blueprint" for your database tables
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Students table structure
            modelBuilder.Entity<Student>(entity =>
            {
                // Primary Key: The unique identifier for each student
                entity.HasKey(e => e.Id);

                // Email column: Required, max 255 characters
                // IsRequired() = NOT NULL in SQL
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);

                // FirstName column: Required, max 100 characters
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);

                // LastName column: Required, max 100 characters
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);

                // Major column: Required, max 100 characters
                entity.Property(e => e.Major).IsRequired().HasMaxLength(100);

                // GPA column: Decimal with 3 total digits, 2 after decimal (like 3.85)
                entity.Property(e => e.GPA).HasColumnType("decimal(3,2)");
            });

            // Configure the Courses table structure
            modelBuilder.Entity<Course>(entity =>
            {
                // Primary Key: The unique identifier for each course
                entity.HasKey(e => e.Id);

                // Course Code: Required, max 20 characters (like "CSIT 340")
                entity.Property(e => e.Code).IsRequired().HasMaxLength(20);

                // Course Title: Required, max 200 characters
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);

                // Description: Optional, max 1000 characters
                entity.Property(e => e.Description).HasMaxLength(1000);

                // Department: Required, max 100 characters
                entity.Property(e => e.Department).IsRequired().HasMaxLength(100);

                // Prerequisites: This is tricky! We store a LIST as a single string in the database
                // HasConversion tells EF Core how to convert between List<string> and string
                entity.Property(e => e.Prerequisites)
                    .HasConversion(
                        // When saving TO database: Convert List to comma-separated string
                        // Example: ["CSIT 220", "MATH 225"] becomes "CSIT 220,MATH 225"
                        v => string.Join(',', v),

                        // When reading FROM database: Convert string back to List
                        // Example: "CSIT 220,MATH 225" becomes ["CSIT 220", "MATH 225"]
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    );
            });
        }
    }
}