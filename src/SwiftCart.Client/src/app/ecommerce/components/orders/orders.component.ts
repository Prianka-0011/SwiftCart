import { Component, inject } from '@angular/core';
import { OrderService } from '../../services/order.service';
import { Order } from '../../models/order';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-orders',
  imports: [CurrencyPipe, DatePipe, CommonModule, MatCardModule, MatButtonModule, MatIconModule, MatChipsModule, MatDividerModule],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss'
})
export class OrdersComponent {
private orderService = inject(OrderService);
orders:Order[] = [];

ngOnInit(){
  this.orderService.getUserOrders().subscribe({
    next: (orders) => {
      this.orders = orders;
      console.log("user orders",orders);
    },
    error: (error) => console.log(error),
  });
}

getStatusColor(status: string): string {
    switch (status) {
      case 'Pending': return 'bg-slate-100 text-slate-700 border-slate-300';
      case 'Confirmed': return 'bg-blue-50 text-blue-700 border-blue-200';
      case 'Packed' : return 'bg-indigo-50 text-indigo-700 border-indigo-200';
      case 'Shipped': return 'bg-yellow-50 text-yellow-700 border-yellow-200';
      case 'Delivered': return 'accent'; // Material theme color
      case 'Cancelled': return 'warn';
      default: return 'primary';
    }
  }

}
