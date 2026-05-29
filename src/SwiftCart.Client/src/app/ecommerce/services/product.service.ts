import { inject, Injectable } from '@angular/core';
import { environment } from '../../../enviroments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ProductParams } from '../models/productParams';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
 baseUrl = environment.baseUrl;
 private http = inject(HttpClient);
 categories: string[] = [];

 getProducts(productParams: ProductParams){
  let params = new HttpParams();

    if (productParams.categories && productParams.categories.length > 0) {
      productParams.categories.forEach(c => {
        params = params.append('categories', c);
      });
    }
    console.log(params.toString());

    if (productParams.sort) {
      params = params.append('sort', productParams.sort)
    }


    params = params.append('pageSize', productParams.pageSize?.toString() ?? '12');
    params = params.append('page', productParams.page?.toString() ?? '1')
  return this.http.get<any>(this.baseUrl + 'product/getAll', {params});
 }

 getCategories(){
    if (this.categories.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'category/getAll').subscribe({
      next: response => this.categories = response,
    })
  }

  getProduct(id: string) {
    if (!id) throw new Error('getProduct requires an id');
    return this.http.get<any>(this.baseUrl + 'product/' + encodeURIComponent(id));
  }
   
}
