namespace CourseCompass.API.DTOs
{
    // This defines what data we return when someone requests enrollment info
    // It includes the student and course details for easy display
    public class EnrollmentResponseDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string Semester { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Grade { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime EnrollmentDate { get; set; }

        // Nested objects for display - this is what makes the API user-friendly
        public StudentInfoDto Student { get; set; } = null!;
        public CourseInfoDto Course { get; set; } = null!;
    }

    // Simple student info for display in enrollment responses
    public class StudentInfoDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Major { get; set; } = string.Empty;
    }

    // Simple course info for display in enrollment responses
    public class CourseInfoDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Department { get; set; } = string.Empty;
    }
}