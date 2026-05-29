import { computed, inject, Injectable, signal, effect } from '@angular/core';
import { environment } from '../../../enviroments/environment';
import { HttpClient } from '@angular/common/http';
import { Cart, CartItem } from '../models/cart';
import { of } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { UserService } from './user.service';
import { Product } from '../models/product';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  baseUrl = environment.baseUrl;
  private http = inject(HttpClient);
  private userService = inject(UserService);
  cart = signal<Cart | null>(this.loadLocalCart());
  itemCount = computed(() => {
    return this.cart()?.items.reduce((sum, item) => sum + item.quantity, 0);
  });

  totals = computed(() => {
    const cart = this.cart();

    if (!cart) return null;
    const subtotal = cart.items.reduce(
      (sum, item) => sum + item.unitPrice * item.quantity,
      0
    );

    let discountValue = 0;

    const total = subtotal - discountValue;

    return {
      subtotal,
      discount: discountValue,
      total,
    };
  });

  getCart() {
    if (this.userService.currentUser()) {
      return this.http.get<Cart>(this.baseUrl + 'cart', { withCredentials: true }).pipe(
        tap((serverCart) => {
          if (serverCart) {
            this.cart.set(serverCart);
            localStorage.setItem('cart_local', JSON.stringify(serverCart));
            localStorage.setItem('cart_id', serverCart.id);
          }
        }),
        catchError((err) => {
          console.log('getCart error', err);
          return of(this.cart());
        })
      );
    }

    return of(this.cart());
  }


  setCart(cart: Cart): void {
    if (this.userService.currentUser()) {
      this.http.patch<Cart>(this.baseUrl + 'cart/update', cart, { withCredentials: true }).subscribe({
        next: (updatedCart) => {
          this.cart.set(updatedCart);
        },
        error: (err) => {
          if (err?.status === 401) {
            this.updateLocalCart(cart);
          }
          console.log('setCartError', err);
        }
      });
      return;
    }

    this.updateLocalCart(cart);
  }

  addItemToCart(productOrItem: Product | CartItem, quantity = 1) {
    const cart = this.cart() ?? this.createCart();
    let item: CartItem;
    if (this.isProduct(productOrItem)) {
      item = this.mapProductToCartItem(productOrItem as any);
    } else {
      item = productOrItem as CartItem;
    }
    cart.items = this.addOrUpdateItem(cart.items, item, quantity);
    // sync to server when user is authenticated; otherwise update local state
    this.setCart(cart);
     
  }

   removeItemFromCart(productId: string, quantity = 1) {
    const cart = this.cart();
    if (!cart) return;
     const item = cart.items.find((i) => i.productId === productId);
    if (!item) return;
    item.quantity -= quantity;
    if (item.quantity <= 0) {
      cart.items = cart.items.filter((i) => i.productId !== productId);
    }

    if (cart.items.length === 0) {
      if (this.userService.currentUser()) {
        this.setCart({ ...cart, items: [] });
      } else {
        this.deleteLocalState();
      }
    } else {
      this.setCart(cart);
    }
  }

  async deleteCart() {
    const cart = this.cart();
    if (!cart) return;

    cart.items = [];

    if (this.userService.currentUser()) {
        this.setCart({...cart, items:[]});
        return;
    }

    localStorage.removeItem('cart_id');
    localStorage.removeItem('cart_local');
    this.cart.set(null);
  }

  private addOrUpdateItem(items: CartItem[], item: CartItem, quantity: number) {
    const index = items.findIndex((i) => i.productId === item.productId);
    if (index === -1) {
      item.quantity = quantity;
      items.push(item);
    } else {
      items[index].quantity += quantity;
    }
    return items;
  }

  private mapProductToCartItem(product: any): CartItem {
    return {
      productId: product.id,
      productName: product.name,
      unitPrice: product.price,
      quantity: 0,
      images: product.images ?? [],
    };
  }

  private isProduct(item: any): item is Product {
    return (
      item &&
      (item as any).id !== undefined &&
      (item as any).productId === undefined
    );
  }

  private createCart() {
    const cart = new Cart();
    localStorage.setItem('cart_id', cart.id);
    this.cart.set(cart);
    return cart;
  }

  private loadLocalCart(): Cart | null {
    try {
      const raw = localStorage.getItem('cart_local');
      if (!raw) return null;
      const parsed = JSON.parse(raw) as Cart;
      this.cart.set(parsed);
      return parsed;
    } catch {
      return null;
    }
  }

  // load server cart automatically when user logs in
  private loadServerCartOnLogin = effect(() => {
    const user = this.userService.currentUser();
    if (user) {
      this.http.get<Cart>(this.baseUrl + 'cart', { withCredentials: true }).subscribe({
        next: (serverCart) => {
          if (serverCart) {
            this.cart.set(serverCart);
            localStorage.setItem('cart_local', JSON.stringify(serverCart));
            localStorage.setItem('cart_id', serverCart.id);
          }
        },
        error: (err) => {
          console.log('loadServerCart error', err);
        }
      });
    }
  });

  private updateLocalCart(cart: Cart) {
    this.cart.set(cart);
      localStorage.setItem('cart_local', JSON.stringify(cart));
      localStorage.setItem('cart_id', cart.id);
     
  }

  private deleteLocalState() {
    this.cart.set(null);
    localStorage.removeItem('cart_local');
    localStorage.removeItem('cart_id');
  }

}
