namespace CourseCompass.API.DTOs
{
    // DTO = Data Transfer Object
    // This defines exactly what data we need to create an enrollment
    // It ONLY includes the fields the user should provide
    public class CreateEnrollmentDto
    {
        // Which student is enrolling (just the ID, not the full Student object)
        public int StudentId { get; set; }

        // Which course they're enrolling in (just the ID, not the full Course object)
        public int CourseId { get; set; }

        // Which semester ("Fall", "Spring", "Summer")
        public string Semester { get; set; } = string.Empty;

        // Which year (2024, 2025, etc.)
        public int Year { get; set; }

        // Optional: Grade if they're updating an existing enrollment
        public string Grade { get; set; } = "In Progress";

        // Optional: Whether the course is completed
        public bool IsCompleted { get; set; } = false;
    }
}