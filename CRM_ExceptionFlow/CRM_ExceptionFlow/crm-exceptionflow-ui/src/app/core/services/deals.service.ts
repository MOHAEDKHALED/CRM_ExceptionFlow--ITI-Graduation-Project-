import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Deal, DealRequest } from '../models/deal.models';

@Injectable({ providedIn: 'root' })
export class DealsService {
  private readonly baseUrl = `${environment.apiUrl}/deals`;

  constructor(private readonly http: HttpClient) {}

  list(params?: {
    customerId?: number;
    assignedToUserId?: number;
    stage?: string;
  }): Observable<Deal[]> {
    let httpParams = new HttpParams();
    if (params) {
      Object.entries(params).forEach(([key, value]) => {
        if (value !== undefined && value !== null && value !== '') {
          httpParams = httpParams.set(key, String(value));
        }
      });
    }
    return this.http.get<Deal[]>(this.baseUrl, { params: httpParams });
  }

  get(id: number): Observable<Deal> {
    return this.http.get<Deal>(`${this.baseUrl}/${id}`);
  }

  create(payload: DealRequest): Observable<Deal> {
    return this.http.post<Deal>(this.baseUrl, payload);
  }

  update(id: number, payload: DealRequest): Observable<Deal> {
    return this.http.put<Deal>(`${this.baseUrl}/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}

