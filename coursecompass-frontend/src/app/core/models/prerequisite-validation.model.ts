export interface PrerequisiteValidationResult {
  isEligible: boolean;
  missingPrerequisites: string[];
  message: string;
  recommendations?: string[];
}

export interface CourseEligibility {
  courseId: number;
  courseCode: string;
  courseTitle: string;
  isEligible: boolean;
  reason?: string;
}
