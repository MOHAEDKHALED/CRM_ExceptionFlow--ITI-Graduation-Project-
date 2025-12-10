import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Interaction, InteractionRequest } from '../models/interaction.models';

@Injectable({ providedIn: 'root' })
export class InteractionsService {
  private readonly baseUrl = `${environment.apiUrl}/interactions`;

  constructor(private readonly http: HttpClient) {}

  list(customerId?: number): Observable<Interaction[]> {
    let httpParams = new HttpParams();
    if (customerId) {
      httpParams = httpParams.set('customerId', customerId);
    }
    return this.http.get<Interaction[]>(this.baseUrl, { params: httpParams });
  }

  create(payload: InteractionRequest): Observable<Interaction> {
    return this.http.post<Interaction>(this.baseUrl, payload);
  }

  update(id: number, payload: InteractionRequest): Observable<Interaction> {
    return this.http.put<Interaction>(`${this.baseUrl}/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}

