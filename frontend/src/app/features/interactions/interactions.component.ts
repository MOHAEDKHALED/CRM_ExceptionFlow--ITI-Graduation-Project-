import { Component, DestroyRef, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { debounceTime } from 'rxjs/operators';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { InteractionsService } from '../../core/services/interactions.service';
import { Interaction, InteractionRequest } from '../../core/models/interaction.models';
import { CustomersService } from '../../core/services/customers.service';
import { AuthService } from '../../core/services/auth.service';
import { CustomerSummary } from '../../core/models/customer.models';

@Component({
  selector: 'app-interactions',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './interactions.component.html',
  styleUrl: './interactions.component.scss'
})
export class InteractionsComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly interactionsService = inject(InteractionsService);
  private readonly customersService = inject(CustomersService);
  private readonly authService = inject(AuthService);
  private readonly destroyRef = inject(DestroyRef);

  readonly interactions = signal<Interaction[]>([]);
  readonly customers = signal<CustomerSummary[]>([]);
  readonly loading = signal(true);
  readonly showForm = signal(false);
  readonly isEditing = signal(false);
  readonly selectedInteraction = signal<Interaction | null>(null);

  readonly filterForm = this.fb.group({
    customerId: ['']
  });

  readonly interactionForm = this.fb.nonNullable.group({
    type: ['Call', Validators.required],
    subject: ['', Validators.required],
    notes: [''],
    interactionDate: [new Date().toISOString().split('T')[0], Validators.required],
    customerId: [0, Validators.required],
    userId: [0, Validators.required]
  });

  readonly types = ['Call', 'Email', 'Meeting', 'Note', 'Other'];

  getInteractionIcon(type: string): string {
    const iconMap: { [key: string]: string } = {
      'Call': 'fas fa-phone',
      'Email': 'fas fa-envelope',
      'Meeting': 'fas fa-calendar-alt',
      'Note': 'fas fa-sticky-note',
      'Other': 'fas fa-ellipsis-h'
    };
    return iconMap[type] || 'fas fa-comment';
  }

  ngOnInit(): void {
    this.loadInteractions();
    this.loadCustomers();
    const currentUser = this.authService.user();
    if (currentUser) {
      this.interactionForm.patchValue({ userId: currentUser.id });
    }

    this.filterForm.valueChanges
      .pipe(debounceTime(300), takeUntilDestroyed(this.destroyRef))
      .subscribe(() => this.loadInteractions());
  }

  loadInteractions() {
    this.loading.set(true);
    const { customerId } = this.filterForm.getRawValue();
    this.interactionsService
      .list(customerId ? Number(customerId) : undefined)
      .subscribe({
        next: (interactions) => this.interactions.set(interactions),
        error: () => this.interactions.set([]),
        complete: () => this.loading.set(false)
      });
  }

  loadCustomers() {
    this.customersService.list({}).subscribe({
      next: (result) => this.customers.set(result.items)
    });
  }

  openCreateForm() {
    this.isEditing.set(false);
    this.interactionForm.reset();
    const currentUser = this.authService.user();
    if (currentUser) {
      this.interactionForm.patchValue({
        userId: currentUser.id,
        interactionDate: new Date().toISOString().split('T')[0],
        type: 'Call'
      });
    }
    this.showForm.set(true);
  }

  openEditForm(interaction: Interaction) {
    this.isEditing.set(true);
    this.interactionForm.patchValue({
      type: interaction.type,
      subject: interaction.subject,
      notes: interaction.notes || '',
      interactionDate: interaction.interactionDate.split('T')[0],
      customerId: interaction.customerId,
      userId: interaction.userId
    });
    this.showForm.set(true);
  }

  saveInteraction() {
    if (this.interactionForm.invalid) {
      this.interactionForm.markAllAsTouched();
      return;
    }

    const payload: InteractionRequest = this.interactionForm.getRawValue();
    const interactionId = this.selectedInteraction()?.id;

    if (this.isEditing() && interactionId) {
      this.interactionsService.update(interactionId, payload).subscribe({
        next: () => {
          this.loadInteractions();
          this.showForm.set(false);
        }
      });
    } else {
      this.interactionsService.create(payload).subscribe({
        next: () => {
          this.loadInteractions();
          this.showForm.set(false);
        }
      });
    }
  }

  deleteInteraction(id: number) {
    if (confirm('Are you sure you want to delete this interaction?')) {
      this.interactionsService.delete(id).subscribe({
        next: () => {
          this.loadInteractions();
          if (this.selectedInteraction()?.id === id) {
            this.selectedInteraction.set(null);
          }
        }
      });
    }
  }
}

