import { inject, Injectable } from '@angular/core';
import { environment } from '../../../enviroments/environment';
import { HttpClient } from '@angular/common/http';

interface CategoryOption {
  id:  string;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  baseUrl = environment.baseUrl;
  private http = inject(HttpClient);

  getAllCategories() {
    return this.http.get<CategoryOption[]>(this.baseUrl + 'category/getAll', { withCredentials: true });
  }

  getCategoryByName(name: string) {
    return this.http.get<any>(this.baseUrl + 'category/' + encodeURIComponent(name), { withCredentials: true });
  }
  createCategory(CategoryData: { name: string; parentCategoryId: string | null; description: string }) {
    return this.http.post(this.baseUrl + 'category/create', CategoryData, { withCredentials: true });
  }
}
