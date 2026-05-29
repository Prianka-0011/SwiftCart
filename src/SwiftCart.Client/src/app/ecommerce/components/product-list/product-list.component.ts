import { Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/product.service';
import { ProductParams } from '../../models/productParams';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule, MatMenuTrigger } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../models/pagination';
import { Product } from '../../models/product';
import { CartService } from '../../services/cart.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    CommonModule,
    MatListModule,
    MatMenuModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    MatMenuTrigger,
    MatPaginator,
    RouterLink
],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss'],
})
export class ProductListComponent implements OnInit {
  @Input() product?: Product;
  cartService = inject(CartService);
  private productService = inject(ProductService);
  private dialogService = inject(MatDialog);
  productParams = new ProductParams();
  pageSizeOptions = [5, 10, 15, 20];
  pageEvent?: PageEvent;
  products?: Pagination<Product>;
  categories: string[] = [];
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low-High', value: 'price_asc' },
    { name: 'Price: High-Low', value: 'price_desc' },
  ];

  ngOnInit() {
    this.productService.getCategories(), this.getProducts();
  }

  getProducts() {
    this.productService.getProducts(this.productParams).subscribe({
      next: (response) => {
        this.products = response;
        console.log(response);
      },
      error: (error) => {
        console.error('Error fetching products:', error);
      },
    });
  }

  onSortChange(event: any) {
    this.productParams.page = 1;
    const selectedOption = event.options[0];
    if (selectedOption) {
      this.productParams.sort = selectedOption.value;
      this.getProducts();
    }
  }

  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedCategories: this.productParams.categories,
      },
    });
    dialogRef.afterClosed().subscribe({
      next: (result) => {
        if (result) {
          this.productParams.page = 1;

          this.productParams.categories = result.selectedCategories;
          this.getProducts();
        }
      },
    });
  }

  handlePageEvent(event: PageEvent) {
    this.productParams.page = event.pageIndex + 1;
    this.productParams.pageSize = event.pageSize;
    this.getProducts();
  }

  
}
