import { Component, inject, OnInit } from '@angular/core';
 import { Router, RouterLink, RouterOutlet,RouterLinkActive } from '@angular/router';
import { MatDivider } from '@angular/material/divider';
import { MatMenuTrigger, MatMenu, MatMenuItem } from '@angular/material/menu';
import { CartService } from './services/cart.service';
import { UserService } from './services/user.service';
import { MatIcon } from "@angular/material/icon";
import { MatButton } from "@angular/material/button";
import { MatBadge } from "@angular/material/badge";
// import { MatProgressBar } from "@angular/material/progress-bar";

 @Component({
  selector: 'app-ecommerce-layout',
  imports: [
    MatIcon,
    MatButton,
    MatBadge,
    RouterLink,
     RouterOutlet,
    RouterLinkActive,
    // MatProgressBar,
    MatMenuTrigger,
    MatMenu,
    MatDivider,
    MatMenuItem,
  ],
  templateUrl: './ecommerce-layout.component.html',
  styleUrl: './ecommerce-layout.component.scss'
})
export class EcommerceLayoutComponent {

  cartService = inject(CartService);
  userService = inject(UserService);
  private router = inject(Router);

  ngOnInit(): void {
     this.userService.getCurrentUser().subscribe({ next: () => {}, error: () => {} });
  }
   
logout() {
  this.userService.logout().subscribe({
    next: () => {
      this.userService.currentUser.set(null);
      this.router.navigateByUrl('/');
    },
    error: () => {
       this.userService.currentUser.set(null);
      this.router.navigateByUrl('/');
    }
  });
}
}
