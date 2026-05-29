export enum OrderStatus {
  Pending = 'Pending',
  Confirmed = 'Confirmed',
  Packed = 'Packed',
  Shipped = 'Shipped',
  OutForDelivery = 'OutForDelivery',
  Delivered = 'Delivered',
  Canceled = 'Canceled',
  Returned = 'Returned'
}

export interface OrderItem {
	productId: string; // Guid as string
	productName: string;
  images?: string[];
	quantity: number;
	unitPrice: number;
 	lineTotal?: number;
}

export interface Order {
	id: string;
	orderNumber: string;
	userId: string;

	shipToName: string;
	shipStreet: string;
	shipCity: string;
	shipState: string;
	shipCountry: string;
	shipZipCode: string;

	subTotal: number;
	shippingCost: number;
	taxAmount: number;
	totalAmount: number;

	paymentStatus: string;
	orderStatus: string;

	createdAt: string;  

	items: OrderItem[];
}

export interface InitiateOrder{
 clientSecret:string;
 orderId:string;
 totalAmount:number;
 orderNumber:string;
}