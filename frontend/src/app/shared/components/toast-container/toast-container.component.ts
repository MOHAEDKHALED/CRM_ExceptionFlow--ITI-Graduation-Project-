import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { trigger, transition, style, animate } from '@angular/animations';
import { ToastService } from '../../../core/services/toast.service';

@Component({
    selector: 'app-toast-container',
    standalone: true,
    imports: [CommonModule],
    template: `
    <div class="toast-container">
      @for (toast of toastService.activeToasts(); track toast.id) {
        <div 
          class="toast toast-{{ toast.type }}"
          [@slideIn]
          (click)="toast.dismissible && toastService.dismiss(toast.id)">
          <div class="toast-icon">
            @switch (toast.type) {
              @case ('success') { <i class="fas fa-check-circle"></i> }
              @case ('error') { <i class="fas fa-times-circle"></i> }
              @case ('warning') { <i class="fas fa-exclamation-triangle"></i> }
              @case ('info') { <i class="fas fa-info-circle"></i> }
            }
          </div>
          <div class="toast-content">
            <div class="toast-title">{{ toast.title }}</div>
            <div class="toast-message">{{ toast.message }}</div>
          </div>
          @if (toast.dismissible) {
            <button class="toast-close" (click)="toastService.dismiss(toast.id); $event.stopPropagation()">
              <i class="fas fa-times"></i>
            </button>
          }
        </div>
      }
    </div>
  `,
    styleUrls: ['./toast-container.component.scss'],
    animations: [
        trigger('slideIn', [
            transition(':enter', [
                style({ transform: 'translateX(100%)', opacity: 0 }),
                animate('300ms cubic-bezier(0.4, 0, 0.2, 1)', style({ transform: 'translateX(0)', opacity: 1 }))
            ]),
            transition(':leave', [
                animate('200ms ease-in', style({ transform: 'translateX(100%)', opacity: 0 }))
            ])
        ])
    ]
})
export class ToastContainerComponent {
    readonly toastService = inject(ToastService);
}
