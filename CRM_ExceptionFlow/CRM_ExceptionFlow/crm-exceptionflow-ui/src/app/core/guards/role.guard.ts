import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  canActivate(route: any): boolean | UrlTree {
    const user = this.authService.user();
    
    if (!user) {
      return this.router.createUrlTree(['/login']);
    }

    const requiredRoles = route.data?.['roles'] as string[];
    
    if (!requiredRoles || requiredRoles.length === 0) {
      return true;
    }

    if (requiredRoles.includes(user.role)) {
      return true;
    }

    // Redirect based on user role: Admin to users, others to dashboard
    const redirectPath = user.role === 'Admin' ? '/users' : '/dashboard';
    return this.router.createUrlTree([redirectPath]);
  }
}

