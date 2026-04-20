namespace CourseCompass.API.Models
{
    // Enrollment represents the relationship between a student and a course
    // This is like the "registration" record when a student signs up for a class
    public class Enrollment
    {
        // Primary key for this enrollment record
        public int Id { get; set; }

        // Foreign key to Student table - which student is enrolled
        public int StudentId { get; set; }

        // Foreign key to Course table - which course they're taking
        public int CourseId { get; set; }

        // Which semester they're taking it (like "Fall 2024", "Spring 2025")
        public string Semester { get; set; } = string.Empty;

        // What year (2024, 2025, etc.)
        public int Year { get; set; }

        // Current grade in the course ("A", "B+", "C", "In Progress", etc.)
        public string Grade { get; set; } = "In Progress";

        // Whether they've completed the course (true = finished, false = currently taking)
        public bool IsCompleted { get; set; } = false;

        // When they enrolled in this course
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        // Navigation properties - these let Entity Framework connect the relationships
        // Student property will be populated with the actual Student object
        public Student Student { get; set; } = null!;

        // Course property will be populated with the actual Course object  
        public Course Course { get; set; } = null!;
    }
}