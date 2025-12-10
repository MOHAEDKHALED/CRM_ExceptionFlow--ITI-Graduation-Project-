import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly form = this.fb.nonNullable.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    fullName: ['', [Validators.required]],
    role: ['Employee', Validators.required],
    department: [''],
    password: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', Validators.required]
  }, { validators: this.passwordMatchValidator });

  readonly isSubmitting = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly successMessage = signal<string | null>(null);
  showPassword = false;
  showConfirmPassword = false;

  readonly departments = [
    'Sales',
    'Support',
    'Technical',
    'Marketing',
    'Finance',
    'HR'
  ];

  readonly roles = [
    { value: 'Employee', label: 'Employee' },
    { value: 'Manager', label: 'Manager' }
  ];

  passwordMatchValidator(group: any) {
    const password = group.get('password');
    const confirmPassword = group.get('confirmPassword');
    
    if (!password || !confirmPassword) {
      return null;
    }
    
    return password.value === confirmPassword.value ? null : { passwordMismatch: true };
  }

  submit() {
    if (this.form.invalid || this.isSubmitting()) {
      this.form.markAllAsTouched();
      return;
    }

    if (this.form.errors?.['passwordMismatch']) {
      this.errorMessage.set('Passwords do not match');
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);
    this.successMessage.set(null);

    const { confirmPassword, ...registerData } = this.form.getRawValue();

    this.authService.register(registerData).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.successMessage.set('Registration successful! Redirecting to login...');
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: (error) => {
        this.isSubmitting.set(false);
        const message = error?.error?.message || error?.error || 'Registration failed. Please try again.';
        this.errorMessage.set(message);
      }
    });
  }

  get passwordMismatch(): boolean {
    return this.form.errors?.['passwordMismatch'] && 
           this.form.get('confirmPassword')?.touched === true;
  }
}

