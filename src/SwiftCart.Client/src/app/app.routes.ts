import { Routes } from '@angular/router';
// import { authRoutes } from './auth/auth-routing.module';
// import { AuthComponent } from './auth/auth.component';
import { EcommerceLayoutComponent } from './ecommerce/ecommerce-layout.component';
import { ecommerceLayoutRoutes } from './ecommerce/ecommerce-layout-routing.module';
import { LoginComponent } from './ecommerce/components/user/login/login.component';
import { AdminLayoutComponent } from './admin/admin-layout.component';
import { adminLayoutRoutes } from './admin/admin-routing.module';
 

export const routes: Routes = [
  {
    path: '',
    component: EcommerceLayoutComponent,
    children: ecommerceLayoutRoutes
  },
  {
    path:"login",
    component: LoginComponent
  },
 
  // {
  //   path: 'auth',
  //   component: AuthComponent,
  //   children: authRoutes
  // },
  {
    path: 'admin',
    component: AdminLayoutComponent,
    children: adminLayoutRoutes
  },
  {
    path: '**',
    redirectTo: 'home'
  }
];