 import {  Routes } from '@angular/router';
 import { ProductsComponent } from './components/products/products.component';
import { CreateProductComponent } from './components/create-product/create-product.component';
import { OrdersComponent } from './components/orders/orders.component';
import { OrderDetailsComponent } from './components/order-details/order-details.component';
import { authGuard } from '../guards/auth.guard';
import { adminGuard } from '../guards/admin-guard';
import { CreateCategoryComponent } from './components/categories/create-category/create-category.component';

 export const adminLayoutRoutes: Routes =  [
      {path: 'products', component: ProductsComponent},
      {path: 'create-product', component: CreateProductComponent},
      {path: 'orders', component: OrdersComponent , canActivate:[authGuard]},
      {path: 'order/:id', component: OrderDetailsComponent, canActivate:[authGuard ]},
      {path: 'create-category', component: CreateCategoryComponent},
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
   
];

 