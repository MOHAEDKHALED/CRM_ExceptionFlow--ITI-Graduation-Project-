import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly form = this.fb.nonNullable.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  });

  readonly isSubmitting = signal(false);
  readonly errorMessage = signal<string | null>(null);
  showPassword = false;

  submit() {
    if (this.form.invalid || this.isSubmitting()) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    this.authService.login(this.form.getRawValue()).subscribe({
      next: (response) => {
        this.isSubmitting.set(false);
        // Redirect Admin to users page, others to dashboard
        const redirectPath = response.user?.role === 'Admin' ? '/users' : '/dashboard';
        this.router.navigate([redirectPath]);
      },
      error: (error) => {
        this.isSubmitting.set(false);
        let message = 'Unable to login.';
        
        if (error?.error) {
          if (typeof error.error === 'string') {
            message = error.error;
          } else if (error.error?.message) {
            message = error.error.message;
          } else if (error.error?.error) {
            message = error.error.error;
          }
        } else if (error?.status === 401) {
          message = 'Invalid username or password';
        } else if (error?.status === 0) {
          message = 'Cannot connect to server. Please check if the backend is running.';
        } else if (error?.status >= 500) {
          message = 'Server error. Please try again later.';
        }
        
        this.errorMessage.set(message);
      }
    });
  }
}

