import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';

// Angular Material
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';

import { Course } from '../../core/models/course.model';
import { CourseService } from '../../core/services/course.service';

@Component({
  selector: 'app-course-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    MatCardModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatFormFieldModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatTooltipModule
  ],
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.scss']
})
export class CourseListComponent implements OnInit {
  private courseService = inject(CourseService);
  private router = inject(Router);

  courses: Course[] = [];
  filteredCourses: Course[] = [];
  departments: string[] = [];
  selectedDepartment: string = '';
  searchTerm: string = '';
  loading: boolean = false;
  error: string = '';

  // Table columns for Material Table
  displayedColumns: string[] = ['code', 'title', 'credits', 'department', 'prerequisites', 'actions'];

  ngOnInit(): void {
    this.loadCourses();
  }

  loadCourses(): void {
    this.loading = true;
    this.error = '';
    
    this.courseService.getAllCourses().subscribe({
      next: (courses: Course[]) => {
        this.courses = courses;
        this.filteredCourses = courses;
        this.extractDepartments();
        this.loading = false;
      },
      error: (error: any) => {
        this.error = 'Failed to load courses: ' + error;
        this.loading = false;
        console.error('Error loading courses:', error);
      }
    });
  }

  extractDepartments(): void {
    const departmentSet = new Set(this.courses.map(course => course.department));
    this.departments = Array.from(departmentSet).sort();
  }

  onSearchChange(): void {
    this.applyFilters();
  }

  onDepartmentChange(): void {
    this.applyFilters();
  }

  applyFilters(): void {
    this.filteredCourses = this.courses.filter(course => {
      const matchesSearch = !this.searchTerm || 
        course.code.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        course.title.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        course.description.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      const matchesDepartment = !this.selectedDepartment || 
        course.department === this.selectedDepartment;
      
      return matchesSearch && matchesDepartment;
    });
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.selectedDepartment = '';
    this.filteredCourses = this.courses;
  }

  viewCourseDetails(courseId: number): void {
    this.router.navigate(['/courses', courseId]);
  }

  getPrerequisitesDisplay(prerequisites: string[]): string {
    if (!prerequisites || prerequisites.length === 0) {
      return 'None';
    }
    return prerequisites.join(', ');
  }
}
