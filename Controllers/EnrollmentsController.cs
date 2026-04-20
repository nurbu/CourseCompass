using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseCompass.API.Data;
using CourseCompass.API.Models;
using CourseCompass.API.DTOs;

namespace CourseCompass.API.Controllers
{
    // This controller handles student enrollment in courses
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        // Database context for accessing enrollment data
        private readonly CourseCompassDbContext _context;

        // Constructor: Inject the database context
        public EnrollmentsController(CourseCompassDbContext context)
        {
            _context = context;
        }

        // GET: api/enrollments
        // Get all enrollments with student and course details
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentResponseDto>>> GetEnrollments()
        {
            // Get enrollments and convert to DTOs for clean API response
            var enrollments = await _context.Enrollments
                .Include(e => e.Student)  // Join with Students table
                .Include(e => e.Course)   // Join with Courses table
                .Select(e => new EnrollmentResponseDto
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    Semester = e.Semester,
                    Year = e.Year,
                    Grade = e.Grade,
                    IsCompleted = e.IsCompleted,
                    EnrollmentDate = e.EnrollmentDate,
                    Student = new StudentInfoDto
                    {
                        Id = e.Student.Id,
                        Email = e.Student.Email,
                        FirstName = e.Student.FirstName,
                        LastName = e.Student.LastName,
                        Major = e.Student.Major
                    },
                    Course = new CourseInfoDto
                    {
                        Id = e.Course.Id,
                        Code = e.Course.Code,
                        Title = e.Course.Title,
                        Credits = e.Course.Credits,
                        Department = e.Course.Department
                    }
                })
                .ToListAsync();

            return enrollments;
        }

        // GET: api/enrollments/5
        // Get a specific enrollment by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentResponseDto>> GetEnrollment(int id)
        {
            // Find enrollment and convert to DTO
            var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Where(e => e.Id == id)
                .Select(e => new EnrollmentResponseDto
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    Semester = e.Semester,
                    Year = e.Year,
                    Grade = e.Grade,
                    IsCompleted = e.IsCompleted,
                    EnrollmentDate = e.EnrollmentDate,
                    Student = new StudentInfoDto
                    {
                        Id = e.Student.Id,
                        Email = e.Student.Email,
                        FirstName = e.Student.FirstName,
                        LastName = e.Student.LastName,
                        Major = e.Student.Major
                    },
                    Course = new CourseInfoDto
                    {
                        Id = e.Course.Id,
                        Code = e.Course.Code,
                        Title = e.Course.Title,
                        Credits = e.Course.Credits,
                        Department = e.Course.Department
                    }
                })
                .FirstOrDefaultAsync();

            if (enrollment == null)
            {
                return NotFound($"Enrollment with ID {id} not found.");
            }

            return enrollment;
        }

        // GET: api/enrollments/student/5
        // Get all enrollments for a specific student
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentResponseDto>>> GetStudentEnrollments(int studentId)
        {
            // Check if student exists
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return NotFound($"Student with ID {studentId} not found.");
            }

            // Get all enrollments for this student
            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId)
                .OrderBy(e => e.Year)
                .ThenBy(e => e.Semester)
                .Select(e => new EnrollmentResponseDto
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    Semester = e.Semester,
                    Year = e.Year,
                    Grade = e.Grade,
                    IsCompleted = e.IsCompleted,
                    EnrollmentDate = e.EnrollmentDate,
                    Student = new StudentInfoDto
                    {
                        Id = e.Student.Id,
                        Email = e.Student.Email,
                        FirstName = e.Student.FirstName,
                        LastName = e.Student.LastName,
                        Major = e.Student.Major
                    },
                    Course = new CourseInfoDto
                    {
                        Id = e.Course.Id,
                        Code = e.Course.Code,
                        Title = e.Course.Title,
                        Credits = e.Course.Credits,
                        Department = e.Course.Department
                    }
                })
                .ToListAsync();

            return enrollments;
        }
        // GET: api/enrollments/student/5/transcript
        // Get completed courses for a student (their transcript)
        [HttpGet("student/{studentId}/transcript")]
        public async Task<ActionResult<IEnumerable<EnrollmentResponseDto>>> GetStudentTranscript(int studentId)
        {
            // Check if student exists
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return NotFound($"Student with ID {studentId} not found.");
            }

            // Get only completed courses with grades
            var transcript = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .Where(e => e.StudentId == studentId && e.IsCompleted == true)
                .OrderBy(e => e.Year)
                .ThenBy(e => e.Semester)
                .Select(e => new EnrollmentResponseDto
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    Semester = e.Semester,
                    Year = e.Year,
                    Grade = e.Grade,
                    IsCompleted = e.IsCompleted,
                    EnrollmentDate = e.EnrollmentDate,
                    Student = new StudentInfoDto
                    {
                        Id = e.Student.Id,
                        Email = e.Student.Email,
                        FirstName = e.Student.FirstName,
                        LastName = e.Student.LastName,
                        Major = e.Student.Major
                    },
                    Course = new CourseInfoDto
                    {
                        Id = e.Course.Id,
                        Code = e.Course.Code,
                        Title = e.Course.Title,
                        Credits = e.Course.Credits,
                        Department = e.Course.Department
                    }
                })
                .ToListAsync();

            return transcript;
        }

        // POST: api/enrollments
        // Enroll a student in a course using DTO
        [HttpPost]
        public async Task<ActionResult<EnrollmentResponseDto>> PostEnrollment(CreateEnrollmentDto enrollmentDto)
        {
            // Validation: Check if student exists
            var student = await _context.Students.FindAsync(enrollmentDto.StudentId);
            if (student == null)
            {
                return BadRequest($"Student with ID {enrollmentDto.StudentId} not found.");
            }

            // Validation: Check if course exists
            var course = await _context.Courses.FindAsync(enrollmentDto.CourseId);
            if (course == null)
            {
                return BadRequest($"Course with ID {enrollmentDto.CourseId} not found.");
            }

            // Validation: Check if student is already enrolled
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == enrollmentDto.StudentId &&
                                        e.CourseId == enrollmentDto.CourseId &&
                                        e.Semester == enrollmentDto.Semester &&
                                        e.Year == enrollmentDto.Year);

            if (existingEnrollment != null)
            {
                return BadRequest($"Student is already enrolled in {course.Code} for {enrollmentDto.Semester} {enrollmentDto.Year}.");
            }

            // Create the enrollment entity from DTO
            var enrollment = new Enrollment
            {
                StudentId = enrollmentDto.StudentId,
                CourseId = enrollmentDto.CourseId,
                Semester = enrollmentDto.Semester,
                Year = enrollmentDto.Year,
                Grade = enrollmentDto.Grade,
                IsCompleted = enrollmentDto.IsCompleted,
                EnrollmentDate = DateTime.Now
            };

            // Add to database
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            // Return the created enrollment as DTO
            var responseDto = new EnrollmentResponseDto
            {
                Id = enrollment.Id,
                StudentId = enrollment.StudentId,
                CourseId = enrollment.CourseId,
                Semester = enrollment.Semester,
                Year = enrollment.Year,
                Grade = enrollment.Grade,
                IsCompleted = enrollment.IsCompleted,
                EnrollmentDate = enrollment.EnrollmentDate,
                Student = new StudentInfoDto
                {
                    Id = student.Id,
                    Email = student.Email,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Major = student.Major
                },
                Course = new CourseInfoDto
                {
                    Id = course.Id,
                    Code = course.Code,
                    Title = course.Title,
                    Credits = course.Credits,
                    Department = course.Department
                }
            };

            return CreatedAtAction(nameof(GetEnrollment), new { id = enrollment.Id }, responseDto);
        }

        // PUT: api/enrollments/5
        // Update an enrollment (usually to change grade or completion status)
        [HttpPut("{id}")]
        public async Task<ActionResult<EnrollmentResponseDto>> PutEnrollment(int id, CreateEnrollmentDto enrollmentDto)
        {
            // Find the existing enrollment
            var existingEnrollment = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (existingEnrollment == null)
            {
                return NotFound($"Enrollment with ID {id} not found.");
            }

            // Validation: If changing student or course, make sure they exist
            if (enrollmentDto.StudentId != existingEnrollment.StudentId)
            {
                var student = await _context.Students.FindAsync(enrollmentDto.StudentId);
                if (student == null)
                {
                    return BadRequest($"Student with ID {enrollmentDto.StudentId} not found.");
                }
            }

            if (enrollmentDto.CourseId != existingEnrollment.CourseId)
            {
                var course = await _context.Courses.FindAsync(enrollmentDto.CourseId);
                if (course == null)
                {
                    return BadRequest($"Course with ID {enrollmentDto.CourseId} not found.");
                }
            }

            // Update the enrollment properties
            existingEnrollment.StudentId = enrollmentDto.StudentId;
            existingEnrollment.CourseId = enrollmentDto.CourseId;
            existingEnrollment.Semester = enrollmentDto.Semester;
            existingEnrollment.Year = enrollmentDto.Year;
            existingEnrollment.Grade = enrollmentDto.Grade;
            existingEnrollment.IsCompleted = enrollmentDto.IsCompleted;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Check if the enrollment still exists
                if (!_context.Enrollments.Any(e => e.Id == id))
                {
                    return NotFound($"Enrollment with ID {id} not found.");
                }
                else
                {
                    // Re-throw the exception if it's a different concurrency issue
                    throw;
                }
            }

            // Reload the enrollment with updated related data
            var updatedEnrollment = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == id);

            // Return the updated enrollment as DTO
            var responseDto = new EnrollmentResponseDto
            {
                Id = updatedEnrollment!.Id,
                StudentId = updatedEnrollment.StudentId,
                CourseId = updatedEnrollment.CourseId,
                Semester = updatedEnrollment.Semester,
                Year = updatedEnrollment.Year,
                Grade = updatedEnrollment.Grade,
                IsCompleted = updatedEnrollment.IsCompleted,
                EnrollmentDate = updatedEnrollment.EnrollmentDate,
                Student = new StudentInfoDto
                {
                    Id = updatedEnrollment.Student.Id,
                    Email = updatedEnrollment.Student.Email,
                    FirstName = updatedEnrollment.Student.FirstName,
                    LastName = updatedEnrollment.Student.LastName,
                    Major = updatedEnrollment.Student.Major
                },
                Course = new CourseInfoDto
                {
                    Id = updatedEnrollment.Course.Id,
                    Code = updatedEnrollment.Course.Code,
                    Title = updatedEnrollment.Course.Title,
                    Credits = updatedEnrollment.Course.Credits,
                    Department = updatedEnrollment.Course.Department
                }
            };

            return responseDto;
        }

        // DELETE: api/enrollments/5
        // Remove a student from a course (withdraw)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);

            if (enrollment == null)
            {
                return NotFound($"Enrollment with ID {id} not found.");
            }

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}