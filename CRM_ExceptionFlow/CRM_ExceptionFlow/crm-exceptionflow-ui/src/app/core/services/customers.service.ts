import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult } from '../models/common';
import { CustomerDetail, CustomerRequest, CustomerSummary } from '../models/customer.models';

@Injectable({ providedIn: 'root' })
export class CustomersService {
  private readonly baseUrl = `${environment.apiUrl}/customers`;

  constructor(private readonly http: HttpClient) {}

  list(params: {
    searchTerm?: string;
    status?: string;
    pageNumber?: number;
    pageSize?: number;
  }): Observable<PagedResult<CustomerSummary>> {
    let httpParams = new HttpParams();
    Object.entries(params).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') {
        httpParams = httpParams.set(key, String(value));
      }
    });
    return this.http.get<PagedResult<CustomerSummary>>(this.baseUrl, { params: httpParams });
  }

  get(id: number): Observable<CustomerDetail> {
    return this.http.get<CustomerDetail>(`${this.baseUrl}/${id}`);
  }

  create(payload: CustomerRequest): Observable<CustomerSummary> {
    return this.http.post<CustomerSummary>(this.baseUrl, payload);
  }

  update(id: number, payload: CustomerRequest): Observable<CustomerSummary> {
    return this.http.put<CustomerSummary>(`${this.baseUrl}/${id}`, payload);
  }

  delete(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}

