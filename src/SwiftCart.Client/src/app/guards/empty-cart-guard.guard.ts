import { CanActivateFn, Router } from '@angular/router';
import { CartService } from '../ecommerce/services/cart.service';
import { inject } from '@angular/core';
import { SnackbarService } from '../ecommerce/services/snackbar.service';

export const emptyCartGuardGuard: CanActivateFn = (route, state) => {
 const cartService = inject(CartService);
 const router = inject(Router);
 const snack = inject(SnackbarService);
 
 if(!cartService.cart() || cartService.cart()?.items.length ===0 || cartService.cart()?.items===undefined){
  snack.error("your cart is empty");
  router.navigateByUrl('/products');
  return false
 };
 return true


};
