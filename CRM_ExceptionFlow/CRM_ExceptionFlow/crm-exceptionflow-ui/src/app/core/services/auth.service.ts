import { Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { LoginRequest, LoginResponse, RegisterRequest, User } from '../models/auth.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly tokenKey = 'crm_token';
  private readonly userKey = 'crm_user';
  private readonly _user = signal<User | null>(null);

  readonly user = computed(() => this._user());
  readonly isAuthenticated = computed(() => !!this._user());

  constructor(private readonly http: HttpClient, private readonly router: Router) {
    this.restoreSession();
  }

  login(credentials: LoginRequest) {
    return this.http
      .post<LoginResponse>(`${environment.apiUrl}/auth/login`, credentials)
      .pipe(tap((response) => this.setSession(response)));
  }

  register(request: RegisterRequest) {
    return this.http.post<User>(`${environment.apiUrl}/auth/register`, request);
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
    this._user.set(null);
    this.router.navigate(['/login']);
  }

  get token(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  private setSession(response: LoginResponse) {
    localStorage.setItem(this.tokenKey, response.token);
    localStorage.setItem(this.userKey, JSON.stringify(response.user));
    this._user.set(response.user);
  }

  private restoreSession() {
    const storedUser = localStorage.getItem(this.userKey);
    if (storedUser) {
      this._user.set(JSON.parse(storedUser) as User);
    }
  }
}

