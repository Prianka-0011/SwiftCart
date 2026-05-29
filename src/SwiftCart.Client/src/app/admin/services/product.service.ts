import { inject, Injectable } from '@angular/core';
import { environment } from '../../../enviroments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ProductParams } from '../../ecommerce/models/productParams';
 

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  baseUrl = environment.baseUrl;
  private http = inject(HttpClient);

  createProduct(formData: FormData) {
    return this.http.post(this.baseUrl + 'product', formData, { withCredentials: true });
  }
  getProducts(productParams: ProductParams) {
    let params = new HttpParams();

    if (productParams.categories && productParams.categories.length > 0) {
      productParams.categories.forEach(c => params = params.append('categories', c));
    }

    if (productParams.sort) params = params.append('sort', productParams.sort);

    params = params.append('page', (productParams.page ?? 1).toString());
    params = params.append('pageSize', (productParams.pageSize ?? 12).toString());

    return this.http.get<any>(this.baseUrl + 'product/getAll', { params, withCredentials: true });
  }

  updateProduct(id: string, payload: any) {
    if (!id) throw new Error('updateProduct requires id');
    return this.http.patch(this.baseUrl + 'product/' + id, payload, { withCredentials: true });
  }
}
