import { Component,  inject, input,   output,   signal } from '@angular/core';
 
import { Review } from '../../models/review';
import { CommonModule, NgClass, NgFor } from '@angular/common';
import { ReviewModalComponent } from './review-modal/review-modal.component';
import { ReviewService } from '../../services/review.service';

@Component({
  selector: 'app-review',
  imports: [CommonModule ,ReviewModalComponent],
  templateUrl: './review.component.html',
  styleUrl: './review.component.scss'
})
export class ReviewComponent {
   
  reviews =  input<Review[]>();
  rates = []
 isOpen =  signal(false);
 submitReview =  output<{ rating: number; comment: string }>();
 

  onOpenDialog(isOpen: boolean){
    
    this.isOpen.set(isOpen);
     
  }


  constructor() {
    console.log("Review items:", this.reviews()?.length);
     
  }

  onSubmitReview(data: { rating: number; comment: string }) {
  this.submitReview.emit(data);
   
  }
}
