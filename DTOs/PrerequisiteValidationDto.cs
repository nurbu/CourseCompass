namespace CourseCompass.API.DTOs
{
    // Request: Check if a student can take a specific course
    public class PrerequisiteCheckDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }

    // Response: Result of prerequisite validation
    public class PrerequisiteValidationResultDto
    {
        public bool CanEnroll { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseTitle { get; set; } = string.Empty;
        public List<string> RequiredPrerequisites { get; set; } = new();
        public List<string> CompletedPrerequisites { get; set; } = new();
        public List<string> MissingPrerequisites { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }

    // Get all courses a student is eligible for
    public class EligibleCoursesDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public List<CourseEligibilityDto> EligibleCourses { get; set; } = new();
        public List<CourseEligibilityDto> IneligibleCourses { get; set; } = new();
    }

    // Individual course eligibility info
    public class CourseEligibilityDto
    {
        public int CourseId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }
        public bool CanEnroll { get; set; }
        public List<string> MissingPrerequisites { get; set; } = new();
        public string Reason { get; set; } = string.Empty;
    }
}