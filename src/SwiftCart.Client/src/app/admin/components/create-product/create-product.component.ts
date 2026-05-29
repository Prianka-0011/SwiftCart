import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { CategoryService } from '../../services/category.service';
import { SnackbarService } from '../../../ecommerce/services/snackbar.service';

@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.component.html',
  styleUrls: ['./create-product.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
})
export class CreateProductComponent {
  private fb = inject(FormBuilder);
  private productService = inject(ProductService);
  private router = inject(Router);
  snack = inject(SnackbarService);
  private categoryService = inject(CategoryService);

  form = this.fb.group({
    name: ['', Validators.required],
    description: [''],
    sku: [''],
    price: [0, [Validators.required, Validators.min(0)]],
    discountPrice: [null],
    stockQuantity: [0, [Validators.min(0)]],
    categoryId: ['', Validators.required],
  });

  images: File[] = [];
  imagePreviews: { name: string; dataUrl: string }[] = [];
  categories: any[] = [];

  onFilesSelected(e: Event) {
    const input = e.target as HTMLInputElement;
    if (!input.files) return;
    const files = Array.from(input.files);
    for (const f of files) {
      this.images.push(f);
      const reader = new FileReader();
      reader.onload = (ev) => {
        this.imagePreviews.push({
          name: f.name,
          dataUrl: ev.target?.result as string,
        });
      };
      reader.readAsDataURL(f);
    }
  }

  removeImage(index: number) {
    const preview = this.imagePreviews[index];
    if (!preview) return;
    this.imagePreviews.splice(index, 1);
    const fileIndex = this.images.findIndex((x) => x.name === preview.name);
    if (fileIndex >= 0) this.images.splice(fileIndex, 1);
  }

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const selectedCategoryName = this.form.value.categoryId ?? '';
    console.log("category " ,selectedCategoryName);
    const formData = new FormData();
    formData.append('Name', this.form.value.name ?? '');
    formData.append('Description', this.form.value.description ?? '');
    formData.append('SKU', this.form.value.sku ?? '');
    formData.append('Price', String(this.form.value.price ?? 0));
    if (this.form.value.discountPrice != null)
      formData.append('DiscountPrice', String(this.form.value.discountPrice));
    formData.append(
      'StockQuantity',
      String(this.form.value.stockQuantity ?? 0)
    );
    formData.append('CategoryId', selectedCategoryName);

    this.images.forEach((file) => formData.append('ImageFiles', file));

    this.productService.createProduct(formData).subscribe({
      next: (res: any) => {
        console.log('Product created with id: ' + (res?.id ?? res));
        this.snack.success('Product created with id: ' + (res?.id ?? res));
        this.router.navigate(['/admin/products']);
      },
      error: (err) => {
        console.error(err);
        this.snack.error('Failed to create product');
      },
    });
  }

 ngOnInit(): void {
  this.categoryService.getAllCategories().subscribe({
    next: (list) => {
      this.categories = list ?? [];
      console.log("Categories:", list);
    },
    error: () => (this.categories = []),
  });
}
}
