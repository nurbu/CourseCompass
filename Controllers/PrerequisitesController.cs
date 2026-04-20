using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseCompass.API.Data;
using CourseCompass.API.DTOs;

namespace CourseCompass.API.Controllers
{
    // This controller handles prerequisite validation and course eligibility
    [ApiController]
    [Route("api/[controller]")]
    public class PrerequisitesController : ControllerBase
    {
        private readonly CourseCompassDbContext _context;

        public PrerequisitesController(CourseCompassDbContext context)
        {
            _context = context;
        }

        // POST: api/prerequisites/validate
        // Check if a student can enroll in a specific course
        [HttpPost("validate")]
        public async Task<ActionResult<PrerequisiteValidationResultDto>> ValidatePrerequisites(PrerequisiteCheckDto request)
        {
            // Get the student
            var student = await _context.Students.FindAsync(request.StudentId);
            if (student == null)
            {
                return NotFound($"Student with ID {request.StudentId} not found.");
            }

            // Get the course with its prerequisites
            var course = await _context.Courses.FindAsync(request.CourseId);
            if (course == null)
            {
                return NotFound($"Course with ID {request.CourseId} not found.");
            }

            // Get all completed courses for this student
            var completedCourses = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == request.StudentId && e.IsCompleted == true)
                .Select(e => e.Course.Code)
                .ToListAsync();

            // Check which prerequisites are completed vs missing
            var completedPrereqs = course.Prerequisites.Where(prereq => completedCourses.Contains(prereq)).ToList();
            var missingPrereqs = course.Prerequisites.Where(prereq => !completedCourses.Contains(prereq)).ToList();

            // Determine if student can enroll
            bool canEnroll = missingPrereqs.Count == 0;

            // Create response
            var result = new PrerequisiteValidationResultDto
            {
                CanEnroll = canEnroll,
                CourseCode = course.Code,
                CourseTitle = course.Title,
                RequiredPrerequisites = course.Prerequisites,
                CompletedPrerequisites = completedPrereqs,
                MissingPrerequisites = missingPrereqs,
                Message = canEnroll
                    ? $"Student can enroll in {course.Code}"
                    : $"Student cannot enroll in {course.Code}. Missing prerequisites: {string.Join(", ", missingPrereqs)}"
            };

            return result;
        }

        // GET: api/prerequisites/student/5/eligible-courses
        // Get all courses a student is eligible to take
        [HttpGet("student/{studentId}/eligible-courses")]
        public async Task<ActionResult<EligibleCoursesDto>> GetEligibleCourses(int studentId)
        {
            // Get the student
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return NotFound($"Student with ID {studentId} not found.");
            }

            // Get all completed courses for this student
            var completedCourses = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId && e.IsCompleted == true)
                .Select(e => e.Course.Code)
                .ToListAsync();

            // Get all currently enrolled courses (to avoid suggesting courses they're already taking)
            var currentlyEnrolled = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId && e.IsCompleted == false)
                .Select(e => e.Course.Code)
                .ToListAsync();

            // Get all courses and check eligibility
            var allCourses = await _context.Courses.ToListAsync();
            var eligibleCourses = new List<CourseEligibilityDto>();
            var ineligibleCourses = new List<CourseEligibilityDto>();

            foreach (var course in allCourses)
            {
                // Skip if already completed or currently enrolled
                if (completedCourses.Contains(course.Code) || currentlyEnrolled.Contains(course.Code))
                {
                    continue;
                }

                // Check prerequisites
                var missingPrereqs = course.Prerequisites.Where(prereq => !completedCourses.Contains(prereq)).ToList();
                bool canEnroll = missingPrereqs.Count == 0;

                var courseEligibility = new CourseEligibilityDto
                {
                    CourseId = course.Id,
                    Code = course.Code,
                    Title = course.Title,
                    Credits = course.Credits,
                    CanEnroll = canEnroll,
                    MissingPrerequisites = missingPrereqs,
                    Reason = canEnroll
                        ? "All prerequisites completed"
                        : $"Missing: {string.Join(", ", missingPrereqs)}"
                };

                if (canEnroll)
                {
                    eligibleCourses.Add(courseEligibility);
                }
                else
                {
                    ineligibleCourses.Add(courseEligibility);
                }
            }

            // Create response
            var result = new EligibleCoursesDto
            {
                StudentId = studentId,
                StudentName = $"{student.FirstName} {student.LastName}",
                EligibleCourses = eligibleCourses.OrderBy(c => c.Code).ToList(),
                IneligibleCourses = ineligibleCourses.OrderBy(c => c.Code).ToList()
            };

            return result;
        }

        // GET: api/prerequisites/course/5/eligible-students
        // Get all students who are eligible to take a specific course
        [HttpGet("course/{courseId}/eligible-students")]
        public async Task<ActionResult<IEnumerable<object>>> GetEligibleStudents(int courseId)
        {
            // Get the course
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound($"Course with ID {courseId} not found.");
            }

            // Get all students
            var students = await _context.Students.ToListAsync();
            var eligibleStudents = new List<object>();

            foreach (var student in students)
            {
                // Get completed courses for this student
                var completedCourses = await _context.Enrollments
                    .Include(e => e.Course)
                    .Where(e => e.StudentId == student.Id && e.IsCompleted == true)
                    .Select(e => e.Course.Code)
                    .ToListAsync();

                // Check if already enrolled in this course
                var alreadyEnrolled = await _context.Enrollments
                    .AnyAsync(e => e.StudentId == student.Id && e.CourseId == courseId);

                if (alreadyEnrolled)
                {
                    continue; // Skip students already enrolled
                }

                // Check prerequisites
                var missingPrereqs = course.Prerequisites.Where(prereq => !completedCourses.Contains(prereq)).ToList();
                bool canEnroll = missingPrereqs.Count == 0;

                if (canEnroll)
                {
                    eligibleStudents.Add(new
                    {
                        StudentId = student.Id,
                        Name = $"{student.FirstName} {student.LastName}",
                        Email = student.Email,
                        Major = student.Major,
                        GPA = student.GPA
                    });
                }
            }

            return eligibleStudents.OrderBy(s => ((dynamic)s).Name).ToList();
        }

        // GET: api/prerequisites/course/5/requirements
        // Get detailed prerequisite information for a course
        [HttpGet("course/{courseId}/requirements")]
        public async Task<ActionResult<object>> GetCourseRequirements(int courseId)
        {
            // Get the course
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound($"Course with ID {courseId} not found.");
            }

            // Get details about each prerequisite course
            var prerequisiteDetails = new List<object>();

            foreach (var prereqCode in course.Prerequisites)
            {
                var prereqCourse = await _context.Courses
                    .FirstOrDefaultAsync(c => c.Code == prereqCode);

                if (prereqCourse != null)
                {
                    prerequisiteDetails.Add(new
                    {
                        Code = prereqCourse.Code,
                        Title = prereqCourse.Title,
                        Credits = prereqCourse.Credits,
                        Department = prereqCourse.Department
                    });
                }
            }

            return new
            {
                Course = new
                {
                    Code = course.Code,
                    Title = course.Title,
                    Credits = course.Credits,
                    Department = course.Department,
                    Description = course.Description
                },
                Prerequisites = prerequisiteDetails,
                HasPrerequisites = course.Prerequisites.Any(),
                PrerequisiteCount = course.Prerequisites.Count
            };
        }
    }
}