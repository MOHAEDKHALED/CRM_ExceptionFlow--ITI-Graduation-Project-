import { Component, DestroyRef, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { debounceTime } from 'rxjs/operators';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CustomersService } from '../../core/services/customers.service';
import { CustomerDetail, CustomerSummary, CustomerRequest } from '../../core/models/customer.models';
import { AuthService } from '../../core/services/auth.service';
import { UsersService } from '../../core/services/users.service';
import { User } from '../../core/models/auth.models';

@Component({
  selector: 'app-customers',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './customers.component.html',
  styleUrl: './customers.component.scss'
})
export class CustomersComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly customersService = inject(CustomersService);
  private readonly destroyRef = inject(DestroyRef);

  private readonly authService = inject(AuthService);
  private readonly usersService = inject(UsersService);

  readonly customers = signal<CustomerSummary[]>([]);
  readonly users = signal<User[]>([]);
  readonly loading = signal(true);
  readonly selectedCustomer = signal<CustomerDetail | null>(null);
  readonly showForm = signal(false);
  readonly isEditing = signal(false);
  
  readonly filterForm = this.fb.group({
    search: [''],
    status: ['']
  });

  readonly customerForm = this.fb.nonNullable.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phone: [''],
    company: [''],
    address: [''],
    status: ['Active', Validators.required],
    assignedToUserId: [0, Validators.required]
  });

  readonly statuses = ['Active', 'Inactive', 'Churned'];

  ngOnInit(): void {
    this.loadCustomers();
    this.loadUsers();
    this.filterForm.valueChanges
      .pipe(debounceTime(300), takeUntilDestroyed(this.destroyRef))
      .subscribe(() => this.loadCustomers());
  }

  loadUsers() {
    this.usersService.list().subscribe({
      next: (users) => this.users.set(users.filter(u => u.isActive))
    });
  }

  loadCustomers() {
    this.loading.set(true);
    const { search, status } = this.filterForm.getRawValue();
    this.customersService
      .list({ searchTerm: search ?? undefined, status: status ?? undefined })
      .subscribe({
        next: (result) => this.customers.set(result.items),
        error: () => this.customers.set([]),
        complete: () => this.loading.set(false)
      });
  }

  selectCustomer(customer: CustomerSummary) {
    this.customersService.get(customer.id).subscribe({
      next: (detail) => this.selectedCustomer.set(detail),
      error: () => this.selectedCustomer.set(null)
    });
  }

  openCreateForm() {
    this.isEditing.set(false);
    this.customerForm.reset();
    this.customerForm.patchValue({
      status: 'Active',
      assignedToUserId: this.authService.user()?.id || 0
    });
    this.showForm.set(true);
  }

  openEditForm(customer: CustomerDetail) {
    this.isEditing.set(true);
    this.customerForm.patchValue({
      name: customer.name,
      email: customer.email,
      phone: customer.phone || '',
      company: customer.company || '',
      address: customer.address || '',
      status: customer.status,
      assignedToUserId: customer.assignedToUserId
    });
    this.showForm.set(true);
  }

  saveCustomer() {
    if (this.customerForm.invalid) {
      this.customerForm.markAllAsTouched();
      return;
    }

    const payload: CustomerRequest = this.customerForm.getRawValue();
    const customerId = this.selectedCustomer()?.id;

    if (this.isEditing() && customerId) {
      this.customersService.update(customerId, payload).subscribe({
        next: () => {
          this.loadCustomers();
          this.showForm.set(false);
          if (this.selectedCustomer()) {
            this.selectCustomer({ id: customerId } as CustomerSummary);
          }
        }
      });
    } else {
      this.customersService.create(payload).subscribe({
        next: () => {
          this.loadCustomers();
          this.showForm.set(false);
        }
      });
    }
  }

  deleteCustomer(id: number) {
    if (confirm('Are you sure you want to delete this customer?')) {
      this.customersService.delete(id).subscribe({
        next: () => {
          this.loadCustomers();
          if (this.selectedCustomer()?.id === id) {
            this.selectedCustomer.set(null);
          }
        }
      });
    }
  }

  get canEdit(): boolean {
    const user = this.authService.user();
    return user?.role === 'Admin' || user?.role === 'Manager';
  }
}

