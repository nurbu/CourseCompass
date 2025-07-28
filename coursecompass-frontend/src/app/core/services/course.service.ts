import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Course } from '../models/course.model';
import { PrerequisiteValidationResult } from '../models/prerequisite-validation.model';

@Injectable({
  providedIn: 'root'
})
export class CourseService {
  constructor(private apiService: ApiService) {}

  // Get all courses
  getAllCourses(): Observable<Course[]> {
    return this.apiService.get<Course[]>('courses');
  }

  // Get single course by ID
  getCourse(id: number): Observable<Course> {
    return this.apiService.get<Course>(`courses/${id}`);
  }

  // Search courses (if you add this endpoint to backend)
  searchCourses(searchTerm: string): Observable<Course[]> {
    return this.apiService.get<Course[]>(`courses/search?term=${searchTerm}`);
  }

  // Get courses by department
  getCoursesByDepartment(department: string): Observable<Course[]> {
    return this.apiService.get<Course[]>(`courses/department/${department}`);
  }

  // Validate prerequisites for a student
  validatePrerequisites(studentId: number, courseId: number): Observable<PrerequisiteValidationResult> {
    return this.apiService.post<PrerequisiteValidationResult>('prerequisites/validate', {
      studentId,
      courseId
    });
  }

  // Get eligible courses for a student
  getEligibleCourses(studentId: number): Observable<any> {
    return this.apiService.get<any>(`prerequisites/student/${studentId}/eligible-courses`);
  }

  // Get course requirements
  getCourseRequirements(courseId: number): Observable<any> {
    return this.apiService.get<any>(`prerequisites/course/${courseId}/requirements`);
  }
}
