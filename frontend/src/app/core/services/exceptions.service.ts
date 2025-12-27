import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult } from '../models/common';
import {
  ExceptionDetail,
  ExceptionRequest,
  ExceptionSummary,
  Recommendation
} from '../models/exception.models';

@Injectable({ providedIn: 'root' })
export class ExceptionsService {
  private readonly baseUrl = `${environment.apiUrl}/exceptions`;

  constructor(private readonly http: HttpClient) {}

  list(params: {
    status?: string;
    priority?: string;
    assignedToUserId?: number;
    pageNumber?: number;
    pageSize?: number;
  }): Observable<PagedResult<ExceptionSummary>> {
    let httpParams = new HttpParams();
    Object.entries(params).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') {
        httpParams = httpParams.set(key, String(value));
      }
    });
    return this.http.get<PagedResult<ExceptionSummary>>(this.baseUrl, { params: httpParams });
  }

  get(id: number): Observable<ExceptionDetail> {
    return this.http.get<ExceptionDetail>(`${this.baseUrl}/${id}`);
  }

  create(payload: ExceptionRequest): Observable<ExceptionSummary> {
    return this.http.post<ExceptionSummary>(this.baseUrl, payload);
  }

  update(id: number, payload: ExceptionRequest): Observable<ExceptionSummary> {
    return this.http.put<ExceptionSummary>(`${this.baseUrl}/${id}`, payload);
  }

  getRecommendations(id: number): Observable<Recommendation[]> {
    return this.http.get<Recommendation[]>(`${this.baseUrl}/${id}/recommendations`);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}

