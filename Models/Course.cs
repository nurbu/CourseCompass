namespace CourseCompass.API.Models
{
    // Course model represents a course in our database
    // Each property becomes a column in the Courses table
    public class Course
    {
        // Primary key - unique identifier for each course
        public int Id { get; set; }

        // Course code like "CSIT 340" - required field
        public string Code { get; set; } = string.Empty;

        // Course title like "Data Structures and Algorithms" - required field
        public string Title { get; set; } = string.Empty;

        // Number of credits (like 3 or 4) - required field
        public int Credits { get; set; }

        // Course description - optional field
        public string Description { get; set; } = string.Empty;

        // Department like "Computer Science" - required field
        public string Department { get; set; } = string.Empty;

        // List of prerequisite course codes - can be empty
        // Example: ["CSIT 220", "MATH 225"]
        public List<string> Prerequisites { get; set; } = new();
    }
}