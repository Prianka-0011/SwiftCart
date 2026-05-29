export interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  discountPrice?: number;
  sku: string;
  stockQuantity: number;
  categoryId: string;
  category: { name: string };
  color?: string;
  size?: string;
  weight?: string;
  
  images: string[];
  reviews?: ProductReview[];
}

export interface ProductImage {
  id: string;
  imageUrl: string;
  isPrimary: boolean;
}

export interface ProductReview {
  id: string;
  reviewerName: string;
  comment: string;
  rating: number;
  createdAt: Date;
}