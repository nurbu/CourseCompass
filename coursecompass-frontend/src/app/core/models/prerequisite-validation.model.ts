export interface PrerequisiteValidationResult {
  canEnroll: boolean;
  courseCode: string;
  courseTitle: string;
  requiredPrerequisites: string[];
  completedPrerequisites: string[];
  missingPrerequisites: string[];
  message: string;
}

export interface PrerequisiteCheckRequest {
  studentId: number;
  courseId: number;
}
