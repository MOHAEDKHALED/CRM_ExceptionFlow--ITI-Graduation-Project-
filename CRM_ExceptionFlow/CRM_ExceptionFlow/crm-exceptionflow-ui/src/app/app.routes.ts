import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login.component';
import { RegisterComponent } from './features/auth/register.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { CustomersComponent } from './features/customers/customers.component';
import { ExceptionsComponent } from './features/exceptions/exceptions.component';
import { DealsComponent } from './features/deals/deals.component';
import { InteractionsComponent } from './features/interactions/interactions.component';
import { UsersComponent } from './features/users/users.component';
import { AuthGuard } from './core/guards/auth.guard';
import { RoleGuard } from './core/guards/role.guard';
import { RedirectComponent } from './core/components/redirect.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: '',
    canActivate: [AuthGuard],
    children: [
      { 
        path: 'dashboard', 
        component: DashboardComponent,
        canActivate: [RoleGuard],
        data: { roles: ['Manager', 'Employee'] }
      },
      { 
        path: 'customers', 
        component: CustomersComponent,
        canActivate: [RoleGuard],
        data: { roles: ['Manager'] }
      },
      { 
        path: 'deals', 
        component: DealsComponent,
        canActivate: [RoleGuard],
        data: { roles: ['Manager'] }
      },
      { 
        path: 'interactions', 
        component: InteractionsComponent,
        canActivate: [RoleGuard],
        data: { roles: ['Manager', 'Employee'] }
      },
      { 
        path: 'exceptions', 
        component: ExceptionsComponent,
        canActivate: [RoleGuard],
        data: { roles: ['Manager', 'Employee'] }
      },
      { 
        path: 'users', 
        component: UsersComponent,
        canActivate: [RoleGuard],
        data: { roles: ['Admin'] }
      },
      { 
        path: '', 
        pathMatch: 'full',
        component: RedirectComponent,
        canActivate: [AuthGuard]
      }
    ]
  },
  { path: '**', component: RedirectComponent }
];
