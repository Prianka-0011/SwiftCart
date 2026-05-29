import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { UserService } from '../../services/user.service';
import { CartService } from '../../services/cart.service';
import { CheckoutService } from '../../services/checkout.service';
import { OrderSummaryComponent } from '../../shared/compoments/order-summary/order-summary.component';
import { MatStepperModule } from '@angular/material/stepper';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { SnackbarService } from '../../services/snackbar.service';
import { StripePaymentService } from '../../services/stripe.service';
import { InitiateOrder } from '../../models/order';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatStepperModule, 
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatCheckboxModule,
    OrderSummaryComponent
  ],
  styleUrls: ['./checkout.component.scss'],
  templateUrl: './checkout.component.html'
})
export class CheckoutComponent   {
   
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private stripeService = inject(StripePaymentService);
  private userService = inject(UserService);
  private checkoutService = inject(CheckoutService);
  snake =  inject(SnackbarService);
  
  cartService = inject(CartService);
 
  currentStep = signal<1 | 2>(1);  
  isLoading = signal(false);
  errorMessage = signal<string | null>(null);
 
  orderData = signal<InitiateOrder | null>(null);
 
  form = this.fb.group({
    shipToName: ['', Validators.required],
    street: ['', Validators.required],
    city: ['', Validators.required],
    state: ['', Validators.required],
    country: ['', Validators.required],
    zipCode: ['', Validators.required]
  });
 
   initiateOrder() {
    if (this.form.invalid) return;

    this.isLoading.set(true);
    this.errorMessage.set(null);

    const dto = this.form.value  

    this.checkoutService.initiateOrder(dto).subscribe({
      next: (res ) => {
        const data = res as InitiateOrder;
        this.orderData.set( data );
 
        this.currentStep.set(2);
        
        setTimeout(() => {
          this.stripeService.initializePaymentElement(data.clientSecret, 'payment-element');
          this.isLoading.set(false);
        }, 100);
      },
      error: (err) => {
        console.error(err);
        this.errorMessage.set('Failed to create order. Please try again.');
        this.isLoading.set(false);
      }
    });
  }

  
  async handlePayment() {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    const result = await this.stripeService.confirmPaymentAndGetId(window.location.origin + '/checkout');

    if (!result.success) {
      this.errorMessage.set(result.error || 'Payment failed');
      this.isLoading.set(false);
      return;
    }

    const orderId = this.orderData()?.orderId;
    if (orderId && result.transactionId) {
      this.checkoutService.confirmPayment({
        orderId: orderId,
        transactionId: result.transactionId
      }).subscribe({
        next: () => {
          this.snake.success('Payment successful!');
          this.isLoading.set(false);
          this.router.navigate(['/checkout/success', orderId]); 
        },
        error: () => {
          this.errorMessage.set('Payment successful, but server confirmation failed. Please contact support.');
          this.isLoading.set(false);
        }
      });
    }
  }

  onStepChange(event: StepperSelectionEvent) {
    if (event.selectedIndex === 1) {
      this.initiateOrder();
    }
  }


}