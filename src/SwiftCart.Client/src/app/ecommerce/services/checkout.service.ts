import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../enviroments/environment';
import { InitiateOrder } from '../models/order';

@Injectable({ providedIn: 'root' })
export class CheckoutService {
  private http = inject(HttpClient);
  baseUrl = environment.baseUrl;

  initiateOrder(payload: any) {
    return this.http.post(
      `${this.baseUrl}order/initiate`,
      payload,
      { withCredentials: true }
    );
  }

  confirmPayment(dto: { orderId: string;  transactionId: string }) {
    return this.http.post(`${this.baseUrl}order/confirm-payment`, dto, { withCredentials: true });
  }
}
