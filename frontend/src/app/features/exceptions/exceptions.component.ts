import { Component, DestroyRef, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { debounceTime } from 'rxjs/operators';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ExceptionsService } from '../../core/services/exceptions.service';
import { ExceptionDetail, ExceptionSummary, Recommendation, ExceptionRequest } from '../../core/models/exception.models';
import { AuthService } from '../../core/services/auth.service';
import { UsersService } from '../../core/services/users.service';
import { User } from '../../core/models/auth.models';
import { ToastService } from '../../core/services/toast.service';

@Component({
  selector: 'app-exceptions',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './exceptions.component.html',
  styleUrl: './exceptions.component.scss'
})
export class ExceptionsComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly exceptionsService = inject(ExceptionsService);
  private readonly authService = inject(AuthService);
  private readonly usersService = inject(UsersService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly toastService = inject(ToastService);

  readonly exceptions = signal<ExceptionSummary[]>([]);
  readonly users = signal<User[]>([]);
  readonly loading = signal(true);
  readonly selected = signal<ExceptionDetail | null>(null);
  readonly recommendations = signal<Recommendation[]>([]);
  readonly loadingRecommendations = signal(false);
  readonly showForm = signal(false);
  readonly isEditing = signal(false);
  readonly saving = signal(false);

  readonly filters = this.fb.group({
    status: ['Open'],
    priority: [''],
    search: ['']
  });

  readonly exceptionForm = this.fb.nonNullable.group({
    projectId: ['', Validators.required],
    module: ['', Validators.required],
    title: ['', Validators.required],
    description: ['', Validators.required],
    stackTrace: [''],
    status: ['Open', Validators.required],
    priority: ['Medium', Validators.required],
    reportedByUserId: [0, Validators.required],
    assignedToUserId: [0],
    resolutionNotes: ['']
  });

  readonly statuses = ['Open', 'In Progress', 'Resolved', 'Closed'];
  readonly priorities = ['High', 'Medium', 'Low'];

  ngOnInit(): void {
    this.loadExceptions();
    this.loadUsers();
    const currentUser = this.authService.user();
    if (currentUser) {
      this.exceptionForm.patchValue({ reportedByUserId: currentUser.id });
    }
    this.filters.valueChanges
      .pipe(debounceTime(300), takeUntilDestroyed(this.destroyRef))
      .subscribe(() => this.loadExceptions());
  }

  loadUsers() {
    this.usersService.list().subscribe({
      next: (users) => this.users.set(users.filter(u => u.isActive))
    });
  }

  loadExceptions() {
    this.loading.set(true);
    const { status, priority } = this.filters.getRawValue();
    this.exceptionsService
      .list({
        status: status ?? undefined,
        priority: priority ?? undefined,
        pageNumber: 1,
        pageSize: 20
      })
      .subscribe({
        next: (result) => this.exceptions.set(result.items),
        error: () => this.exceptions.set([]),
        complete: () => this.loading.set(false)
      });
  }

  selectException(item: ExceptionSummary) {
    this.exceptionsService.get(item.id).subscribe({
      next: (detail) => {
        this.selected.set(detail);
        // Load recommendations if they exist
        if (detail.recommendations && detail.recommendations.length > 0) {
          this.recommendations.set(detail.recommendations);
        } else {
          this.recommendations.set([]);
        }
      },
      error: () => this.selected.set(null)
    });
  }

  loadRecommendations(exceptionId: number) {
    this.loadingRecommendations.set(true);
    this.exceptionsService.getRecommendations(exceptionId).subscribe({
      next: (recs) => {
        this.recommendations.set(recs);
        // Reload exception detail to get updated recommendations
        this.exceptionsService.get(exceptionId).subscribe({
          next: (detail) => {
            this.selected.set(detail);
            // Update recommendations from detail
            if (detail.recommendations && detail.recommendations.length > 0) {
              this.recommendations.set(detail.recommendations);
            }
          }
        });
      },
      error: (error) => {
        console.error('Failed to get AI recommendation:', error);
        this.loadingRecommendations.set(false);
        alert('Failed to get AI recommendation. Please try again.');
      },
      complete: () => this.loadingRecommendations.set(false)
    });
  }

  generateAIRecommendation(exceptionId: number) {
    if (!exceptionId) {
      this.toastService.warning('No Exception Selected', 'Please select an exception first.');
      return;
    }

    if (this.loadingRecommendations()) {
      return; // Prevent multiple simultaneous requests
    }

    this.loadingRecommendations.set(true);

    // Call the AI Recommendation service endpoint
    this.exceptionsService.getRecommendations(exceptionId).subscribe({
      next: (recs) => {
        // Update recommendations
        this.recommendations.set(recs);

        this.toastService.success(
          'AI Recommendation Generated',
          'The AI has analyzed the exception and provided a recommendation.'
        );

        // Reload exception detail to get updated recommendations
        this.exceptionsService.get(exceptionId).subscribe({
          next: (detail) => {
            this.selected.set(detail);
            // Update recommendations from detail
            if (detail.recommendations && detail.recommendations.length > 0) {
              this.recommendations.set(detail.recommendations);
            }
          }
        });
      },
      error: (error) => {
        console.error('Failed to generate AI recommendation:', error);
        const errorMessage = error?.error?.message || error?.message || 'Failed to generate AI recommendation. Please try again.';
        this.toastService.error('AI Recommendation Failed', errorMessage);
        this.loadingRecommendations.set(false);
      },
      complete: () => {
        this.loadingRecommendations.set(false);
      }
    });
  }

  openCreateForm() {
    this.isEditing.set(false);
    this.exceptionForm.reset();
    const currentUser = this.authService.user();
    if (currentUser) {
      this.exceptionForm.patchValue({
        reportedByUserId: currentUser.id,
        status: 'Open',
        priority: 'Medium'
      });
    }
    this.showForm.set(true);
  }

  openEditForm(exception: ExceptionDetail) {
    this.isEditing.set(true);
    this.exceptionForm.patchValue({
      projectId: exception.projectId,
      module: exception.module,
      title: exception.title,
      description: exception.description,
      stackTrace: exception.stackTrace || '',
      status: exception.status,
      priority: exception.priority,
      reportedByUserId: exception.reportedByUserId,
      assignedToUserId: exception.assignedToUserId || 0,
      resolutionNotes: exception.resolutionNotes || ''
    });
    this.showForm.set(true);
  }

  saveException() {
    // Validate form
    if (this.exceptionForm.invalid) {
      this.exceptionForm.markAllAsTouched();
      this.toastService.error(
        'Validation Error',
        'Please fill in all required fields correctly.'
      );
      return;
    }

    // Prevent double submission
    if (this.saving()) {
      return;
    }

    this.saving.set(true);
    const payload: ExceptionRequest = this.exceptionForm.getRawValue();
    const exceptionId = this.selected()?.id;

    const operation$ = this.isEditing() && exceptionId
      ? this.exceptionsService.update(exceptionId, payload)
      : this.exceptionsService.create(payload);

    operation$.subscribe({
      next: () => {
        this.saving.set(false);
        this.showForm.set(false);
        this.loadExceptions();

        this.toastService.success(
          this.isEditing() ? 'Exception Updated' : 'Exception Created',
          this.isEditing()
            ? 'The exception has been updated successfully.'
            : 'The exception has been created successfully.'
        );

        // Reload detail if editing
        if (this.isEditing() && exceptionId) {
          this.selectException({ id: exceptionId } as ExceptionSummary);
        }
      },
      error: (error) => {
        this.saving.set(false);
        console.error('Failed to save exception:', error);

        const errorMessage = error?.error?.message
          || error?.error?.title
          || error?.message
          || 'An unexpected error occurred. Please try again.';

        this.toastService.error(
          'Save Failed',
          errorMessage
        );
      }
    });
  }

  deleteException(id: number) {
    if (!confirm('Are you sure you want to delete this exception? This action cannot be undone.')) {
      return;
    }

    this.exceptionsService.delete(id).subscribe({
      next: () => {
        this.loadExceptions();
        this.selected.set(null);
        this.toastService.success(
          'Exception Deleted',
          'The exception has been deleted successfully.'
        );
      },
      error: (error) => {
        console.error('Failed to delete exception:', error);
        const errorMessage = error?.error?.message || 'Failed to delete exception. Please try again.';
        this.toastService.error('Delete Failed', errorMessage);
      }
    });
  }

  get canEdit(): boolean {
    // جميع الـ roles يمكنهم إضافة وتعديل Exceptions
    return true;
  }

  get canDelete(): boolean {
    // Manager و Employee يمكنهم حذف Exceptions
    const user = this.authService.user();
    return user?.role === 'Manager' || user?.role === 'Employee';
  }
}

