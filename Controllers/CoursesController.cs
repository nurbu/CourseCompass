using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseCompass.API.Data;
using CourseCompass.API.Models;

namespace CourseCompass.API.Controllers
{
    // This attribute tells ASP.NET Core this is an API controller
    [ApiController]
    // This sets the route pattern - /api/courses will come to this controller
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        // Private field to hold our database context
        // The underscore prefix is a C# convention for private fields
        private readonly CourseCompassDbContext _context;

        // Constructor: When this controller is created, inject the database context
        // This is called "Dependency Injection" - ASP.NET Core automatically provides the context
        public CoursesController(CourseCompassDbContext context)
        {
            _context = context;
        }

        // GET: api/courses
        // This method handles GET requests to /api/courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            // Go to the Courses table and get all courses
            // ToListAsync() makes it asynchronous (doesn't block the thread)
            return await _context.Courses.ToListAsync();
        }

        // GET: api/courses/5
        // This method handles GET requests to /api/courses/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            // Find a course by its ID
            var course = await _context.Courses.FindAsync(id);

            // If no course found, return 404 Not Found
            if (course == null)
            {
                return NotFound();
            }

            // Return the course (200 OK with course data)
            return course;
        }

        // POST: api/courses
        // This method handles POST requests to /api/courses (creating new courses)
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            // Add the new course to the context (in memory, not saved yet)
            _context.Courses.Add(course);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return 201 Created with the location of the new course
            // This returns: 201 Created, Location: /api/courses/{id}, Body: course data
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }
    }
}