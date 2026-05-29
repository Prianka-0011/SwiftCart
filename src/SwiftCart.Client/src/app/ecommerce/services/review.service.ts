import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../enviroments/environment';
import { Observer } from '@apollo/client/utilities';
import { Review } from '../models/review';
 

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private http = inject(HttpClient);
baseUrl = environment.baseUrl;

getReviewsByProductId(productId: string) {
    if (!productId) throw new Error('product id is required to get reviews');
    return this.http.get<any>(this.baseUrl+ 'review/' + encodeURIComponent(productId));
}

addReview(reviewData: {productId: string, rating: number, comment: string})  {
    if (!reviewData.productId) throw new Error('product id is required to add review');
    
    console.log("review data", reviewData);
    return this.http.post<Review>(this.baseUrl + 'review', reviewData,{withCredentials: true});
}
 
}
