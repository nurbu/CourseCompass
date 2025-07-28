using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseCompass.API.Data;
using CourseCompass.API.Models;

namespace CourseCompass.API.Controllers
{
    // This controller handles all Student-related API requests
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        // Database context for accessing student data
        private readonly CourseCompassDbContext _context;

        // Constructor: Inject the database context
        public StudentsController(CourseCompassDbContext context)
        {
            _context = context;
        }

        // GET: api/students
        // Get all students from the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            // Return all students as JSON
            return await _context.Students.ToListAsync();
        }

        // GET: api/students/5
        // Get a specific student by their ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            // Find student by ID
            var student = await _context.Students.FindAsync(id);

            // If student doesn't exist, return 404 Not Found
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            // Return the student data
            return student;
        }

        // POST: api/students
        // Create a new student
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            // Basic validation: Check if email already exists
            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.Email == student.Email);

            if (existingStudent != null)
            {
                return BadRequest("A student with this email already exists.");
            }

            // Add the new student to the database context
            _context.Students.Add(student);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return 201 Created with the location of the new student
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // PUT: api/students/5
        // Update an existing student
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            // Check if the ID in the URL matches the student's ID
            if (id != student.Id)
            {
                return BadRequest("Student ID mismatch.");
            }

            // Mark the student as modified
            _context.Entry(student).State = EntityState.Modified;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Check if the student still exists
                if (!StudentExists(id))
                {
                    return NotFound($"Student with ID {id} not found.");
                }
                else
                {
                    // Re-throw the exception if it's a different concurrency issue
                    throw;
                }
            }

            // Return 204 No Content (successful update)
            return NoContent();
        }

        // DELETE: api/students/5
        // Delete a student
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            // Find the student to delete
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            // Remove the student from the database context
            _context.Students.Remove(student);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return 204 No Content (successful deletion)
            return NoContent();
        }

        // Helper method: Check if a student exists
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}