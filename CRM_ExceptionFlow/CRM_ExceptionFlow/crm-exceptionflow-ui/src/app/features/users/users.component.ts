import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { UsersService, CreateUserRequest, UpdateUserRequest } from '../../core/services/users.service';
import { User } from '../../core/models/auth.models';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss'
})
export class UsersComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly usersService = inject(UsersService);

  readonly users = signal<User[]>([]);
  readonly loading = signal(true);
  readonly showForm = signal(false);
  readonly isEditing = signal(false);
  readonly selectedUser = signal<User | null>(null);

  readonly userForm = this.fb.nonNullable.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    fullName: ['', Validators.required],
    role: ['Employee', Validators.required],
    department: [''],
    password: ['', Validators.minLength(8)],
    isActive: [true]
  });

  readonly roles = ['Admin', 'Manager', 'Employee'];
  readonly departments = ['Sales', 'Support', 'Technical', 'Marketing', 'Finance', 'HR', 'IT'];

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.loading.set(true);
    this.usersService.list().subscribe({
      next: (users) => this.users.set(users),
      error: () => this.users.set([]),
      complete: () => this.loading.set(false)
    });
  }

  openCreateForm() {
    this.isEditing.set(false);
    this.userForm.reset();
    this.userForm.patchValue({
      role: 'Employee',
      isActive: true
    });
    this.showForm.set(true);
  }

  openEditForm(user: User) {
    this.isEditing.set(true);
    this.selectedUser.set(user);
    this.userForm.patchValue({
      username: user.username,
      email: user.email,
      fullName: user.fullName,
      role: user.role,
      department: user.department || '',
      isActive: user.isActive
    });
    // Clear password for edit
    this.userForm.get('password')?.setValidators([]);
    this.userForm.get('password')?.updateValueAndValidity();
    this.showForm.set(true);
  }

  saveUser() {
    if (this.userForm.invalid) {
      this.userForm.markAllAsTouched();
      return;
    }

    const formValue = this.userForm.getRawValue();
    const userId = this.selectedUser()?.id;

    if (this.isEditing() && userId) {
      const payload: UpdateUserRequest = {
        fullName: formValue.fullName,
        role: formValue.role,
        department: formValue.department || undefined,
        password: formValue.password || undefined,
        isActive: formValue.isActive
      };
      this.usersService.update(userId, payload).subscribe({
        next: () => {
          this.loadUsers();
          this.showForm.set(false);
        }
      });
    } else {
      const payload: CreateUserRequest = {
        username: formValue.username,
        email: formValue.email,
        fullName: formValue.fullName,
        role: formValue.role,
        department: formValue.department || undefined,
        password: formValue.password,
        isActive: formValue.isActive
      };
      this.usersService.create(payload).subscribe({
        next: () => {
          this.loadUsers();
          this.showForm.set(false);
        }
      });
    }
  }

  deactivateUser(id: number) {
    if (confirm('Are you sure you want to deactivate this user?')) {
      this.usersService.deactivate(id).subscribe({
        next: () => this.loadUsers()
      });
    }
  }
}

