import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { Order, OrderStatus } from '../../../ecommerce/models/order';

// Admin view type matching backend OrdersResponseDto
interface AdminOrder {
  id: string;
  orderNumber: string;
  userId: string;
  createdAt: string;
  totalAmount: number;
  paymentStatus: string;
  orderStatus: string;
}
import { OrderService } from '../../services/order.service';
import { Pagination } from '../../../ecommerce/models/pagination';
 
@Component({
  selector: 'app-orders-table',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatIconModule,
    MatButtonModule,
    MatMenuModule
  ],
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  private orderService = inject(OrderService);
  private router = inject(Router);
  orders?: Pagination<AdminOrder>

  displayedColumns: string[] = ['orderNumber',  'date', 'total', 'paymentStatus', 'orderStatus', 'actions'];
  dataSource = new MatTableDataSource<AdminOrder>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit() {
    this.orderService.getAllOrders().subscribe({
      next: (orders) => {
        this.orders = orders as unknown as Pagination<AdminOrder>;
        this.dataSource.data = (this.orders?.data) ?? [];
      },
      error: (error) => console.log(error),
    });
  }

  viewOrder(orderId: string) {
    if (!orderId) return;
    // Navigate to admin order details. Adjust path if your route differs.
    this.router.navigate(['/admin/order/', orderId]);
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  /**
   * Returns Tailwind classes for the PAYMENT Status badge
   */
  getPaymentStyles(status: string): string {
    switch(status.toLowerCase()) {
      case 'paid': return 'bg-emerald-50 text-emerald-700 border-emerald-200 ring-emerald-600/20';
      case 'pending': return 'bg-amber-50 text-amber-700 border-amber-200 ring-amber-600/20';
      case 'failed': return 'bg-red-50 text-red-700 border-red-200 ring-red-600/20';
      default: return 'bg-slate-50 text-slate-700 border-slate-200 ring-slate-600/20';
    }
  }

  /**
   * Returns Tailwind classes for the ORDER Status badge
   */
  getOrderStatusStyles(status: string): string {
    const s = (status || '').toLowerCase();
    if (s === 'delivered') return 'bg-green-100 text-green-700 border-green-200';
    if (s === 'shipped' || s === 'outfordelivery' || s === 'out_for_delivery') return 'bg-purple-100 text-purple-700 border-purple-200';
    if (s === 'packed' || s === 'confirmed') return 'bg-blue-100 text-blue-700 border-blue-200';
    if (s === 'pending') return 'bg-slate-100 text-slate-600 border-slate-200';
    if (s === 'canceled' || s === 'cancelled') return 'bg-red-100 text-red-700 border-red-200';
    if (s === 'returned') return 'bg-orange-100 text-orange-800 border-orange-200';
    return 'bg-slate-100 text-slate-600';
  }
}