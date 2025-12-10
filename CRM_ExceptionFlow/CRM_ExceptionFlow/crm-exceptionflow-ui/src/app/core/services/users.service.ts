import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { User } from '../models/auth.models';

export interface CreateUserRequest {
  username: string;
  email: string;
  fullName: string;
  role: string;
  department?: string;
  password: string;
  isActive: boolean;
}

export interface UpdateUserRequest {
  fullName: string;
  role: string;
  department?: string;
  password?: string;
  isActive: boolean;
}

@Injectable({ providedIn: 'root' })
export class UsersService {
  private readonly baseUrl = `${environment.apiUrl}/users`;

  constructor(private readonly http: HttpClient) {}

  list(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl);
  }

  get(id: number): Observable<User> {
    return this.http.get<User>(`${this.baseUrl}/${id}`);
  }

  create(payload: CreateUserRequest): Observable<User> {
    return this.http.post<User>(this.baseUrl, payload);
  }

  update(id: number, payload: UpdateUserRequest): Observable<User> {
    return this.http.put<User>(`${this.baseUrl}/${id}`, payload);
  }

  deactivate(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}

