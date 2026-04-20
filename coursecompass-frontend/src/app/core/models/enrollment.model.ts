import { Student } from './student.model';
import { Course } from './course.model';

export interface Enrollment {
  id: number;
  studentId: number;
  courseId: number;
  semester: string;
  year: number;
  grade: string;
  isCompleted: boolean;
  enrollmentDate: Date;
  student?: Student;
  course?: Course;
}
