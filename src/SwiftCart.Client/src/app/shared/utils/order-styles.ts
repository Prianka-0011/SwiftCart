import { OrderStatus } from "../../ecommerce/models/order";

 
export function getOrderStatusStyles(status: OrderStatus): string {
  switch (status) {
    case OrderStatus.Delivered:
      return 'bg-emerald-100 text-emerald-700 border-emerald-200';
    case OrderStatus.Shipped:
    case OrderStatus.OutForDelivery:
      return 'bg-purple-100 text-purple-700 border-purple-200';
    case OrderStatus.Packed:
    case OrderStatus.Confirmed:
      return 'bg-blue-100 text-blue-700 border-blue-200';
    case OrderStatus.Pending:
      return 'bg-amber-100 text-amber-700 border-amber-200';
    case OrderStatus.Canceled:
      return 'bg-red-100 text-red-700 border-red-200';
    case OrderStatus.Returned:
      return 'bg-orange-100 text-orange-800 border-orange-200';
    default:
      return 'bg-slate-100 text-slate-600 border-slate-200';
  }
}

 
export function getPaymentStatusStyles(status: string): string {
  switch (status?.toLowerCase()) {
    case 'paid':
      return 'bg-emerald-50 text-emerald-700 border-emerald-200 ring-emerald-600/20';
    case 'pending':
      return 'bg-amber-50 text-amber-700 border-amber-200 ring-amber-600/20';
    case 'failed':
      return 'bg-red-50 text-red-700 border-red-200 ring-red-600/20';
    default:
      return 'bg-slate-50 text-slate-700 border-slate-200 ring-slate-600/20';
  }
}