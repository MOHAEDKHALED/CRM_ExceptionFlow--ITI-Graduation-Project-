import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-redirect',
  standalone: true,
  template: ''
})
export class RedirectComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  constructor() {
    const user = this.authService.user();
    const redirectPath = user?.role === 'Admin' ? '/users' : '/dashboard';
    this.router.navigate([redirectPath]);
  }
}

