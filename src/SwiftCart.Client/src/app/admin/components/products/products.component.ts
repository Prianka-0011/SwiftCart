import { Component, Inject, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import {  MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
 
import { ProductService } from '../../services/product.service';
import { ProductParams } from '../../../ecommerce/models/productParams';
import { Product } from '../../../ecommerce/models/product';
import { TemplateRef, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SnackbarService } from '../../../ecommerce/services/snackbar.service';
 
 

 

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatPaginatorModule, MatIconModule, MatButtonModule, MatMenuModule, MatDialogModule,   FormsModule, MatFormFieldModule, MatInputModule],
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit {
  private productService = inject(ProductService);
  private router = inject(Router);
  private dialog = inject(MatDialog);
  snack = inject(SnackbarService)
   
  promptValue = '1';

  @ViewChild('confirmDialog', { static: true }) confirmDialog!: TemplateRef<any>;
  @ViewChild('promptDialog', { static: true }) promptDialog!: TemplateRef<any>;

  displayedColumns: string[] = ['id', 'name', 'category', 'price', 'discountPrice', 'stockQuantity', 'actions'];
  dataSource = new MatTableDataSource<Product>([]);

  productParams = new ProductParams();
  pageSizeOptions = [5, 10, 15, 20];
  totalCount = 0;

  openMenuIndex: number | null = null;

  ngOnInit(): void {
    this.loadProducts();
  }

  get page() { return this.productParams.page; }
  get pageSize() { return this.productParams.pageSize; }

  loadProducts() {
    this.productService.getProducts(this.productParams).subscribe({
      next: (res) => {
         
        this.dataSource.data = res?.data  ?? [];
        this.totalCount = res?.totalCount ??  (Array.isArray(res) ? res.length : 0);
      },
      error: (err) => console.error('Failed to load products', err)
    });
  }

  viewProduct(id: string) {
    if (!id) return;
    this.router.navigate(['/product', id]);
  }

  toggleMenu(index: number, e: Event) {
    e.stopPropagation();
    this.openMenuIndex = this.openMenuIndex === index ? null : index;
  }

  onEdit(product: Product) {
    this.router.navigate(['/admin/products/edit', product.id]);
  }

  markAsStockout(product: Product) {
    if (!product?.id) return;
    const ref = this.dialog.open(this.confirmDialog, { data: { title: 'Mark as stockout', message: `Mark product "${product.name}" as out of stock?`, confirmText: 'Mark Out' } });
    ref.afterClosed().subscribe(confirmed => {
      if (!confirmed) return;
      this.productService.updateProduct(product.id, { StockQuantity: 0 }).subscribe({
        next: () => { this.snack.success('Product marked as stockout'); this.loadProducts(); },
        error: (err) => { console.error(err); this.snack.error('Failed to update product' ); }
      });
    });
  }

  addStock(product: Product) {
    if (!product?.id) return;
    this.promptValue = '1';
    const ref = this.dialog.open(this.promptDialog, { data: { title: 'Add stock', label: 'Quantity', value: '1', placeholder: 'Enter quantity' } });
    ref.afterClosed().subscribe(val => {
      if (val == null) return;
      const qty = parseInt(String(val), 10);
      if (isNaN(qty) || qty <= 0) { this.snack.error('Invalid quantity'); return; }
      const newQty = (product.stockQuantity ?? 0) + qty;
      this.productService.updateProduct(product.id, { StockQuantity: newQty }).subscribe({
        next: () => { this.snack.success('Stock updated'); this.loadProducts(); },
        error: (err) => { console.error(err); this.snack.error('Failed to update stock'); }
      });
    });
  }

  updateProductDetails(product: Product) {
    if (!product?.id) return;
    this.router.navigate(['/admin/products/edit', product.id]);
  }

  handlePageEvent(event: PageEvent) {
    this.productParams.page = event.pageIndex + 1;
    this.productParams.pageSize = event.pageSize;
    this.loadProducts();
  }
}
