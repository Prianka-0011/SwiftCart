 


import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatCardModule } from '@angular/material/card';
import { getOrderStatusStyles, getPaymentStatusStyles } from '../../../shared/utils/order-badge-styles.utils';
import { OrderService } from '../../services/order.service';
import { Order } from '../../../ecommerce/models/order';
 

@Component({
  selector: 'app-order-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatCardModule
  ],
   templateUrl: './order-details.component.html',
  styleUrl: './order-details.component.scss'
})
export class OrderDetailsComponent implements OnInit {
  private orderService = inject(OrderService);
  private activatedRoute = inject(ActivatedRoute);
  order?:Order;
 
  readonly getStatusStyles = getOrderStatusStyles;
  readonly getPaymentStyles = getPaymentStatusStyles;

  ngOnInit() {
      this.loadOrderDetails();
  }

  loadOrderDetails() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) return;

    this.orderService.getOrderDetailed(id).subscribe({
      next: (order) => {
        this.order = order;
        console.log("order detail", order);
      },
      error: (error) => {
        console.error('Error fetching order details:', error);
      }
    });
  }

  

 
}