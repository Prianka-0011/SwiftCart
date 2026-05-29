import { Component, inject, Input } from '@angular/core';
import { CartItem } from '../../../models/cart';
import { RouterLink } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { CartService } from '../../../services/cart.service';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-cart-item',
  imports: [RouterLink, MatIconModule, CommonModule, MatButtonModule],
  standalone: true,
  templateUrl: './cart-item.component.html',
  styleUrls: ['./cart-item.component.scss']
})
export class CartItemComponent {
  cartService = inject(CartService);
  @Input() item!: CartItem;

  incrementQuantity() {
    this.cartService.addItemToCart(this.item);
  }

  decrementQuantity() {
    this.cartService.removeItemFromCart(this.item.productId, 1);
  }

  removeItemFromCart() {
    this.cartService.removeItemFromCart(this.item.productId, this.item.quantity);
  }
}
