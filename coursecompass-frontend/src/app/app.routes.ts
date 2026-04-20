import { Routes } from '@angular/router';
import { CourseListComponent } from './features/courses/course-list.component';

export const routes: Routes = [
  { 
    path: '', 
    redirectTo: '/login', 
    pathMatch: 'full' 
  },
  {
    path: 'courses',
    component: CourseListComponent
  },
  {
    path: 'login',
    loadComponent: () => import('./auth/components/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./auth/components/register/register.component').then(m => m.RegisterComponent)
  },
  {
    path: 'forgot-password',
    loadComponent: () => import('./auth/components/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent)
  },
  {
    path: 'dashboard',
    redirectTo: '/courses',
    pathMatch: 'full'
  },
  { 
    path: '**', 
    redirectTo: '/courses' 
  }
];
