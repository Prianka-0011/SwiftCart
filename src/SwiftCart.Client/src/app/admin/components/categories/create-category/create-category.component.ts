import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';

import { CategoryService } from '../../../services/category.service';

interface CategoryOption {
  id: string;
  name: string;
}

@Component({
  selector: 'app-create-category',
  imports: [ReactiveFormsModule],
  templateUrl: './create-category.component.html',
})
export class CreateCategoryComponent implements OnInit {
  categoryService = inject(CategoryService);
  destroyRef = inject(DestroyRef);
  options: CategoryOption[] | [] = [];

  form = new FormGroup({
    name: new FormControl(''),
    parentCategoryId: new FormControl<CategoryOption | null>(null),
    description: new FormControl(''),
  });

  ngOnInit() {
    const subscription = this.categoryService.getAllCategories().subscribe({
      next: (list) => {
        this.options = list ?? [];
        console.log('Categories:', list);
      },
      error: () => (this.options = []),
    });
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  onSubmit() {
    const categoryData = {
      name: this.form.value.name ?? '',
      parentCategoryId: this.form.value.parentCategoryId?.id ?? null,
      description: this.form.value.description ?? '',
    };
    console.log("form",categoryData);
    const subscription = this.categoryService.createCategory(categoryData).subscribe({
      next: (res) => {
        console.log('Category created successfully', res);
        this.form.reset();
      },
      error: (err) => console.error('Error creating category:', err),
    });
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
    
  }
}
