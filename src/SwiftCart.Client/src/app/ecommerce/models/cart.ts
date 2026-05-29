import { nanoid } from "nanoid";

export type CartType = {
  id: string;
  items: CartItem[];
  deliveryMethodId?: number;
  paymentIntentId?: string;
  clientSecret?: string;
}

export type CartItem = {
  productId: string;
  productName?: string;
  unitPrice: number;
  quantity: number;
  images?: string[];
  
}

export class Cart implements CartType {
  id = nanoid();
  items: CartItem[] = [];
  deliveryMethodId?: number;
  paymentIntentId?: string;
  clientSecret?: string;
  coupon?: Coupon;
}

export type Coupon = {
    name: string;
    amountOff?: number;
    percentOff?: number;
    promotionCode: string;
    couponId: string;
}