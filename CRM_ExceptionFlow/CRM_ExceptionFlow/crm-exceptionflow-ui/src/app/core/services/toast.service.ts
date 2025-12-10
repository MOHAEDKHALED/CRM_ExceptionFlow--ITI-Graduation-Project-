import { Injectable, signal } from '@angular/core';

export interface Toast {
    id: string;
    type: 'success' | 'error' | 'warning' | 'info';
    title: string;
    message: string;
    duration?: number;
    dismissible?: boolean;
}

@Injectable({
    providedIn: 'root'
})
export class ToastService {
    private readonly toasts = signal<Toast[]>([]);
    private idCounter = 0;

    readonly activeToasts = this.toasts.asReadonly();

    success(title: string, message: string, duration = 5000) {
        this.show({ type: 'success', title, message, duration });
    }

    error(title: string, message: string, duration = 7000) {
        this.show({ type: 'error', title, message, duration });
    }

    warning(title: string, message: string, duration = 6000) {
        this.show({ type: 'warning', title, message, duration });
    }

    info(title: string, message: string, duration = 5000) {
        this.show({ type: 'info', title, message, duration });
    }

    private show(toast: Omit<Toast, 'id'>) {
        const id = `toast-${++this.idCounter}`;
        const newToast: Toast = {
            ...toast,
            id,
            dismissible: true
        };

        this.toasts.update(toasts => [...toasts, newToast]);

        if (toast.duration && toast.duration > 0) {
            setTimeout(() => this.dismiss(id), toast.duration);
        }
    }

    dismiss(id: string) {
        this.toasts.update(toasts => toasts.filter(t => t.id !== id));
    }

    clear() {
        this.toasts.set([]);
    }
}
