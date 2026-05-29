import { Component, inject, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { UserService } from '../../services/user.service';
import { CartItemComponent } from './cart-item/cart-item.component';
import { EmptyStateComponent } from '../../shared/compoments/empty-state/empty-state.component';
import { OrderSummaryComponent } from '../../shared/compoments/order-summary/order-summary.component';
import { Router } from '@angular/router';
import { NgFor, NgIf } from '@angular/common';
 
@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CartItemComponent, EmptyStateComponent, OrderSummaryComponent, NgFor, NgIf],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent {
  cartService = inject(CartService);
  userService = inject(UserService);
  private router = inject(Router);
  cart:any[] = []

  ngOnInit(): void {
 
    
     this.userService.getCurrentUser().subscribe({
      next: () => {
        if (this.userService.currentUser()) {
          this.cartService.getCart().subscribe({ next: () => {
            this.cart = (this.cartService.cart() ?? []) as any[];
            console.log('cart', this.cart);
          }, error: () => {} });
        }

      },
      error: () => {}
    });
  }

  

  onAction() {
    this.router.navigateByUrl('/products');
  }

  trackByProduct(index: number, item: any) {
    return item?.productId;
  }

}
