import { inject, Injectable } from '@angular/core';
import { environment } from '../../../enviroments/environment';
import { HttpClient } from '@angular/common/http';
import { Order } from '../models/order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
 baseUrl = environment.baseUrl;
  private http = inject(HttpClient);

  getUserOrders() {
    return this.http.get<Order[]>(this.baseUrl + 'order', { withCredentials: true });
  }
}
