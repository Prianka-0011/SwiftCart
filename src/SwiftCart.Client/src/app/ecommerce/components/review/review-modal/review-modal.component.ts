import { NgClass, NgForOf } from '@angular/common';
import { Component,input, output } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
 

export interface ReviewSubmitData {
  rating: number;
  comment: string;
}
@Component({
  selector: 'app-review-modal',
  imports: [ReactiveFormsModule, NgClass, NgForOf],
  templateUrl: './review-modal.component.html',
  styleUrl: './review-modal.component.scss',
})
export class ReviewModalComponent {
   
   
  isOpen = input<boolean>(false);
  closeDialog = output<void>();
  submitReview = output<ReviewSubmitData>();

  reviewForm: FormGroup;
  stars = [1, 2, 3, 4, 5];
  

  constructor(private fb: FormBuilder) {
    this.reviewForm = this.fb.group({
      rating: [0, [Validators.required, Validators.min(1)]],
      comment: ['', [Validators.required, Validators.minLength(10)]],
    });
  }

  setRating(rating: number) {
    this.reviewForm.patchValue({ rating });
  }

  onSubmit() {
    if (this.reviewForm.valid) {
      this.submitReview.emit(this.reviewForm.value);
      this.onCloseDialog();
    }
  }
  onCloseDialog() {
    this.reviewForm.reset({ rating: 0, comment: '' });
    this.closeDialog.emit();
  }
}
