import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { ActivatedRoute, Router } from '@angular/router';
 import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, MatCard, MatFormField, MatInput, MatButton, MatLabel],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private router = inject(Router);
  private activatedRoute = inject(ActivatedRoute);
  returnUrl = '/products';

  constructor() {
    const url = this.activatedRoute.snapshot.queryParams['returnUrl'];
    if (url) this.returnUrl = url;
  }

  loginForm = this.fb.group({
    email: [''],
    password: ['']
  })

  onSubmit() {
    console.log('Login form submitted', this.loginForm.value);
    this.userService.login(this.loginForm.value).subscribe({
      next: (user) => {
         this.router.navigateByUrl(this.returnUrl);
      },
      error: (err) => {
        console.error('Login failed', err);
       }
    });
  }
}
