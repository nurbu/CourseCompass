using CourseCompass.API.Data;
using CourseCompass.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseCompass.API.Services
{
    // Service to populate the database with sample data for testing
    public class SeedDataService
    {
        private readonly CourseCompassDbContext _context;

        public SeedDataService(CourseCompassDbContext context)
        {
            _context = context;
        }

        // Method to seed the database with sample courses from Montclair State
        public async Task SeedDataAsync()
        {
            // Only seed if database is empty
            if (await _context.Courses.AnyAsync() || await _context.Students.AnyAsync())
            {
                return; // Database already has data
            }

            // Sample Montclair State Computer Science courses
            var courses = new List<Course>
            {
                new Course
                {
                    Code = "CSIT 104",
                    Title = "Introduction to Programming",
                    Credits = 3,
                    Description = "Introduction to programming concepts using Java programming language.",
                    Department = "Computer Science",
                    Prerequisites = new List<string>()
                },
                new Course
                {
                    Code = "CSIT 111",
                    Title = "Computer Programming I",
                    Credits = 4,
                    Description = "Fundamental programming concepts including variables, control structures, and basic object-oriented programming.",
                    Department = "Computer Science",
                    Prerequisites = new List<string> { "CSIT 104" }
                },
                new Course
                {
                    Code = "CSIT 220",
                    Title = "Computer Programming II",
                    Credits = 4,
                    Description = "Advanced programming concepts including inheritance, polymorphism, and data structures.",
                    Department = "Computer Science",
                    Prerequisites = new List<string> { "CSIT 111" }
                },
                new Course
                {
                    Code = "CSIT 340",
                    Title = "Data Structures and Algorithms",
                    Credits = 4,
                    Description = "Study of fundamental data structures and algorithms including analysis of time and space complexity.",
                    Department = "Computer Science",
                    Prerequisites = new List<string> { "CSIT 220", "MATH 225" }
                },
                new Course
                {
                    Code = "CSIT 355",
                    Title = "Database Systems",
                    Credits = 3,
                    Description = "Introduction to database design, SQL, and database management systems.",
                    Department = "Computer Science",
                    Prerequisites = new List<string> { "CSIT 220" }
                },
                new Course
                {
                    Code = "CSIT 425",
                    Title = "Software Engineering",
                    Credits = 4,
                    Description = "Software development methodologies, project management, and team-based software development.",
                    Department = "Computer Science",
                    Prerequisites = new List<string> { "CSIT 340", "CSIT 355" }
                },
                new Course
                {
                    Code = "CSIT 490",
                    Title = "Senior Capstone Project",
                    Credits = 4,
                    Description = "Culminating project demonstrating mastery of computer science concepts.",
                    Department = "Computer Science",
                    Prerequisites = new List<string> { "CSIT 425" }
                },
                new Course
                {
                    Code = "MATH 225",
                    Title = "Calculus I",
                    Credits = 4,
                    Description = "Limits, derivatives, and applications of differential calculus.",
                    Department = "Mathematics",
                    Prerequisites = new List<string>()
                },
                new Course
                {
                    Code = "MATH 226",
                    Title = "Calculus II",
                    Credits = 4,
                    Description = "Integration techniques, infinite series, and applications of integral calculus.",
                    Department = "Mathematics",
                    Prerequisites = new List<string> { "MATH 225" }
                },
                new Course
                {
                    Code = "ENGL 110",
                    Title = "College Writing I",
                    Credits = 3,
                    Description = "Development of writing skills through composition and critical reading.",
                    Department = "English",
                    Prerequisites = new List<string>()
                },
                new Course
                {
                    Code = "PHYS 121",
                    Title = "General Physics I",
                    Credits = 4,
                    Description = "Mechanics, heat, and wave motion with laboratory.",
                    Department = "Physics",
                    Prerequisites = new List<string> { "MATH 225" }
                }
            };

            // Sample students
            var students = new List<Student>
            {
                new Student
                {
                    Email = "john.smith@montclair.edu",
                    FirstName = "John",
                    LastName = "Smith",
                    Major = "Computer Science",
                    StartDate = new DateTime(2022, 9, 1),
                    GPA = 3.45m
                },
                new Student
                {
                    Email = "jane.doe@montclair.edu",
                    FirstName = "Jane",
                    LastName = "Doe",
                    Major = "Computer Science",
                    StartDate = new DateTime(2021, 9, 1),
                    GPA = 3.78m
                },
                new Student
                {
                    Email = "mike.johnson@montclair.edu",
                    FirstName = "Mike",
                    LastName = "Johnson",
                    Major = "Information Technology",
                    StartDate = new DateTime(2023, 1, 15),
                    GPA = 3.22m
                }
            };

            // Add data to context
            _context.Courses.AddRange(courses);
            _context.Students.AddRange(students);

            // Save to database
            await _context.SaveChangesAsync();
        }
    }
}