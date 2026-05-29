import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CurrencyPipe, NgFor, NgIf } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatDivider } from '@angular/material/divider';
import { MatIcon } from '@angular/material/icon';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { CartService } from '../../services/cart.service';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product';
import { ReviewService } from '../../services/review.service';
import { ReviewComponent } from '../review/review.component';

@Component({
  selector: 'app-product-details',
  imports: [MatIcon, CurrencyPipe, NgIf, NgFor, ReviewComponent],
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss'],
})
export class ProductDetailComponent implements OnInit {
  private productService = inject(ProductService);
  private reviewService = inject(ReviewService);
  private cartService = inject(CartService);
  private activatedRoute = inject(ActivatedRoute);
  private destroyRef = inject(DestroyRef);

  reviews = signal<any[]>([]);
  product?: Product;

  quantityInCart = 0;
  quantity = 1;
  selectedImage = 0;
  productId = this.activatedRoute.snapshot.paramMap.get('id');

  ngOnInit() {
    this.loadProduct();
    this.loadReviews();
  }

  loadProduct() {
    if (!this.productId) return;
    const subscription = this.productService
      .getProduct(this.productId)
      .subscribe({
        next: (product) => {
          this.product = product;
          this.updateQuantityInBasket();
        },
        error: (error) => console.log(error),
      });
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  loadReviews() {
    if (!this.productId) return;
    const subscription = this.reviewService
      .getReviewsByProductId(this.productId)
      .subscribe({
        next: (reviews) => {
          this.reviews.set(reviews);
          console.log('review', reviews);
        },
        error: (error) => console.log(error),
      });
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  addReview(data: { rating: number; comment: string }) {
    if (!this.productId) return;
    const reviewData = { ...data, productId: this.productId };
    this.reviewService.addReview(reviewData).subscribe({
      next: (response) => {
        this.loadReviews();
      },
      error: (error) => {
        console.error('Error adding review', error);
      },
    });
  }

  updateQuantityInBasket() {
    this.quantityInCart =
      this.cartService
        .cart()
        ?.items.find((item) => item.productId === this.product?.id)?.quantity ||
      0;
    this.quantity = this.quantityInCart || 1;
  }

  getButtonText() {
    return this.quantityInCart > 0 ? 'Update Cart' : 'Add to Cart';
  }
  changeImage(index: number) {
    this.selectedImage = index;
  }
}
