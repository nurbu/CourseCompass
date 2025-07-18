namespace CourseCompass.API.Models
{
    // Student model represents a student record in our database
    // Each property becomes a column in the Students table
    public class Student
    {
        // Primary key - unique identifier for each student
        public int Id { get; set; }

        // Student's email address - required field
        public string Email { get; set; } = string.Empty;

        // Student's first name - required field
        public string FirstName { get; set; } = string.Empty;

        // Student's last name - required field
        public string LastName { get; set; } = string.Empty;

        // Student's major (like "Computer Science") - required field
        public string Major { get; set; } = string.Empty;

        // When the student started college
        public DateTime StartDate { get; set; }

        // Student's current GPA (like 3.85)
        public decimal GPA { get; set; }
    }
}