import { Component, DestroyRef, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { debounceTime } from 'rxjs/operators';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DealsService } from '../../core/services/deals.service';
import { Deal, DealRequest } from '../../core/models/deal.models';
import { CustomersService } from '../../core/services/customers.service';
import { AuthService } from '../../core/services/auth.service';
import { CustomerSummary } from '../../core/models/customer.models';

@Component({
  selector: 'app-deals',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './deals.component.html',
  styleUrl: './deals.component.scss'
})
export class DealsComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly dealsService = inject(DealsService);
  private readonly customersService = inject(CustomersService);
  private readonly authService = inject(AuthService);
  private readonly destroyRef = inject(DestroyRef);

  readonly deals = signal<Deal[]>([]);
  readonly customers = signal<CustomerSummary[]>([]);
  readonly loading = signal(true);
  readonly selectedDeal = signal<Deal | null>(null);
  readonly showForm = signal(false);
  readonly isEditing = signal(false);

  readonly filterForm = this.fb.group({
    customerId: [''],
    stage: ['']
  });

  readonly dealForm = this.fb.nonNullable.group({
    title: ['', Validators.required],
    description: [''],
    amount: [0, [Validators.required, Validators.min(0)]],
    stage: ['Prospecting', Validators.required],
    priority: ['Medium', Validators.required],
    customerId: [0, Validators.required],
    assignedToUserId: [0, Validators.required],
    expectedCloseDate: ['']
  });

  readonly stages = ['Prospecting', 'Qualification', 'Proposal', 'Negotiation', 'Closed Won', 'Closed Lost'];
  readonly priorities = ['High', 'Medium', 'Low'];

  ngOnInit(): void {
    this.loadDeals();
    this.loadCustomers();
    const currentUser = this.authService.user();
    if (currentUser) {
      this.dealForm.patchValue({ assignedToUserId: currentUser.id });
    }

    this.filterForm.valueChanges
      .pipe(debounceTime(300), takeUntilDestroyed(this.destroyRef))
      .subscribe(() => this.loadDeals());
  }

  loadDeals() {
    this.loading.set(true);
    const { customerId, stage } = this.filterForm.getRawValue();
    this.dealsService
      .list({
        customerId: customerId ? Number(customerId) : undefined,
        stage: stage ?? undefined
      })
      .subscribe({
        next: (deals) => this.deals.set(deals),
        error: () => this.deals.set([]),
        complete: () => this.loading.set(false)
      });
  }

  loadCustomers() {
    this.customersService.list({}).subscribe({
      next: (result) => this.customers.set(result.items)
    });
  }

  selectDeal(deal: Deal) {
    this.selectedDeal.set(deal);
  }

  openCreateForm() {
    this.isEditing.set(false);
    this.dealForm.reset();
    const currentUser = this.authService.user();
    if (currentUser) {
      this.dealForm.patchValue({ 
        assignedToUserId: currentUser.id,
        stage: 'Prospecting',
        priority: 'Medium'
      });
    }
    this.showForm.set(true);
  }

  openEditForm(deal: Deal) {
    this.isEditing.set(true);
    this.dealForm.patchValue({
      title: deal.title,
      description: deal.description || '',
      amount: deal.amount,
      stage: deal.stage,
      priority: deal.priority,
      customerId: deal.customerId,
      assignedToUserId: deal.assignedToUserId,
      expectedCloseDate: deal.expectedCloseDate || ''
    });
    this.showForm.set(true);
  }

  saveDeal() {
    if (this.dealForm.invalid) {
      this.dealForm.markAllAsTouched();
      return;
    }

    const payload: DealRequest = this.dealForm.getRawValue();
    const dealId = this.selectedDeal()?.id;

    if (this.isEditing() && dealId) {
      this.dealsService.update(dealId, payload).subscribe({
        next: () => {
          this.loadDeals();
          this.showForm.set(false);
        }
      });
    } else {
      this.dealsService.create(payload).subscribe({
        next: () => {
          this.loadDeals();
          this.showForm.set(false);
        }
      });
    }
  }

  deleteDeal(id: number) {
    if (confirm('Are you sure you want to delete this deal?')) {
      this.dealsService.delete(id).subscribe({
        next: () => {
          this.loadDeals();
          if (this.selectedDeal()?.id === id) {
            this.selectedDeal.set(null);
          }
        }
      });
    }
  }
}

