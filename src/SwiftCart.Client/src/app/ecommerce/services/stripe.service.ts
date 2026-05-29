import { inject, Injectable } from '@angular/core';
import { environment } from '../../../enviroments/environment';
 
import {
  Stripe,
  StripeElements,
  loadStripe,
} from '@stripe/stripe-js';

@Injectable({ providedIn: 'root' })
export class StripePaymentService {
  private stripePromise = loadStripe(environment.stripePublishableKey);
  private stripe: Stripe | null = null;
  private elements: StripeElements | null = null;

  async initializePaymentElement(clientSecret: string, mountElementId: string) {
    this.stripe = await this.stripePromise;
    if (!this.stripe) throw new Error('Stripe failed to load');

    
    this.elements = this.stripe.elements({ 
      clientSecret, 
      appearance: { theme: 'stripe' } 
    });

     
    const paymentElement = this.elements.create('payment');
    paymentElement.mount(`#${mountElementId}`);
  }

  
  async confirmPaymentAndGetId(returnUrl: string): Promise<{ success: boolean; transactionId?: string; error?: string }> {
    if (!this.stripe || !this.elements) return { success: false, error: 'Stripe not initialized' };

    const result = await this.stripe.confirmPayment({
      elements: this.elements,
      confirmParams: {
        return_url: returnUrl,  
      },
      redirect: 'if_required'  
    });

    if (result.error) {
      return { success: false, error: result.error.message };
    } 
    
    if (result.paymentIntent && result.paymentIntent.status === 'succeeded') {
      return { success: true, transactionId: result.paymentIntent.id };
    }

    return { success: false, error: 'Unexpected payment status' };
  }
}