import { Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { HomeComponent } from './components/home/home.component';
import { ProductDetailComponent } from './components/product-detail/product-detail.component';
import { CartComponent } from './components/cart/cart.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { authGuard } from '../guards/auth.guard';
import { emptyCartGuardGuard } from '../guards/empty-cart-guard.guard';
import { CheckoutSuccessComponent } from './components/checkout/checkout-success/checkout-success.component';
import { CheckoutFailComponent } from './components/checkout/checkout-fail/checkout-fail.component';
import { OrdersComponent } from './components/orders/orders.component';


export const ecommerceLayoutRoutes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'products', component: ProductListComponent },
  { path: 'product/:id', component: ProductDetailComponent },
  {path: 'cart', component: CartComponent},
  {path: 'checkout', component: CheckoutComponent, canActivate: [authGuard,emptyCartGuardGuard]},
  { path: 'checkout/success/:orderId', component: CheckoutSuccessComponent },
  { path: 'checkout/fail', component: CheckoutFailComponent },
  {path:'orders',component:OrdersComponent,canActivate:[authGuard]},
  // { path: '**', redirectTo: 'home' },
];
