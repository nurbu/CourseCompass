import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Auth, sendPasswordResetEmail } from '@angular/fire/auth';
import { FirebaseError } from 'firebase/app';
import { LogoComponent } from '../../../shared/components/logo/logo.component';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, LogoComponent],
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly auth = inject(Auth);

  readonly isLoading = signal<boolean>(false);
  readonly errorMessage = signal<string>('');
  readonly successMessage = signal<string>('');

  readonly form = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email]]
  });

  async sendResetEmail(): Promise<void> {
    if (this.form.invalid || this.isLoading()) {
      this.form.markAllAsTouched();
      return;
    }

    const { email } = this.form.getRawValue();

    this.isLoading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    try {
      await sendPasswordResetEmail(this.auth, email);
      this.successMessage.set('Password reset email sent! Check your inbox.');
      this.form.reset();
    } catch (error) {
      this.errorMessage.set(this.humanizeFirebaseError(error));
    } finally {
      this.isLoading.set(false);
    }
  }

  private humanizeFirebaseError(error: unknown): string {
    const defaultMessage = 'Unable to send reset email. Please try again.';

    if (!(error instanceof FirebaseError)) {
      return defaultMessage;
    }

    switch (error.code) {
      case 'auth/user-not-found':
        return 'No account found with this email address.';
      case 'auth/invalid-email':
        return 'The email address is invalid.';
      case 'auth/network-request-failed':
        return 'Network error. Check your connection and try again.';
      default:
        return defaultMessage;
    }
  }
}
