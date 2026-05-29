import { Component, signal, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { User } from '../../../ecommerce/models/user';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    CommonModule, 
    RouterModule, 
    MatListModule, 
    MatIconModule
  ],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent {
  @Input() user?: User | null = null;
   
   menuItems = signal([
     { label: 'Orders', icon: 'shopping_cart', route: '/admin/orders' },
    { label: 'Products', icon: 'inventory_2', route: '/admin/products' },
    {label: 'New Product', icon: 'add_circle', route: '/admin/create-product' },
    {label: 'New Category', icon: 'add_circle', route: '/admin/create-category' },
   
  ]);
}