import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { SidebarComponent } from './shared/sidebar/sidebar.component';
import { UserService } from '../ecommerce/services/user.service';
import { User } from '../ecommerce/models/user';
 
  
@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatSidenavModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    SidebarComponent  
  ],
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.scss'
})
export class AdminLayoutComponent {
  userService = inject(UserService);
  private router = inject(Router);
  user?:User;

  isSidebarOpen = signal(true);

  toggleSidebar() {
    this.isSidebarOpen.update(val => !val);
  }
  ngOnInit(): void {
     this.userService.getCurrentUser().subscribe({ next: (user) => { this.user = user, console.log(user); }, error: () => {} });
  }
   
logout() {
  this.userService.logout().subscribe({
    next: (user) => {
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