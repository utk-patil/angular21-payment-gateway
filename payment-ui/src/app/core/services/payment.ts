import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { Transaction } from '../models/transaction.model';
import { PageResult } from '../models/pagination.model';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  private baseUrl = `${environment.apiBaseUrl}/payments`;

  constructor(private http: HttpClient) {}

  createPayment(orderId: string, amount: number): Observable<{ paymentUrl: string }> {
    return this.http.post<{ paymentUrl: string }>(
      `${this.baseUrl}/create`,
      { orderId, amount }
    );
  }

  getTransactions(pageNumber: number, pageSize: number, search?: string, status?: string): Observable<PageResult<Transaction>> {

    let params = new HttpParams()
    .set('pageNumber', pageNumber)
    .set('pageSize', pageSize);

    if (search && search.trim().length > 0) {
      params = params.set('search', search.trim());
    }

    if (status && status.length > 0) {
      params = params.set('status', status);
    }

    return this.http.get<PageResult<Transaction>>(
      `${this.baseUrl}/transactions`,
      { params }
    );
  }

  confirmPayment(token: string, isSuccess: boolean) {
    return this.http.post<{ redirectUrl: string }>(
      `${this.baseUrl}/confirm`,
      {
        token,
        isSuccess,
      }
    );
  }

  getDemoPaySession(token: string) {
    return this.http.get<{
      orderId: string;
      amount: number;
    }>(
      `${this.baseUrl}/demo-pay/session/${token}`
    );
  }

  repay(orderId: string): Observable<{ paymentUrl: string }> {
    return this.createPayment(orderId, 0);
  }

}