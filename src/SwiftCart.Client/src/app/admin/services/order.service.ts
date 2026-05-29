import { inject, Injectable } from '@angular/core';
import { environment } from '../../../enviroments/environment';
import { HttpClient } from '@angular/common/http';
import { Order } from '../../ecommerce/models/order';
import { Pagination } from '../../ecommerce/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  baseUrl = environment.baseUrl;
  private http = inject(HttpClient);

  getAllOrders() {
    return this.http.get<Pagination<Order>>(this.baseUrl + 'order/all', { withCredentials: true });
  }

  getOrderDetailed(id: string) {
    return this.http.get<Order>(this.baseUrl + 'order/admin/' + id, { withCredentials: true });
  }


   
}
