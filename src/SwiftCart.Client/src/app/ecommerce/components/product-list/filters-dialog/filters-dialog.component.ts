import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../../services/product.service';

@Component({
  selector: 'app-filters-dialog',
  standalone: true,
  imports: [CommonModule, MatDividerModule, MatListModule, MatButtonModule, FormsModule],
  templateUrl: './filters-dialog.component.html',
  styleUrls: ['./filters-dialog.component.scss']
})
export class FiltersDialogComponent implements OnInit {
  private dialogRef = inject(MatDialogRef<FiltersDialogComponent>);
  data = inject(MAT_DIALOG_DATA) as { selectedCategories?: string[]; selectedTypes?: string[] };
  productService = inject(ProductService);

  selectedCategories: string[] = [];
  selectedTypes: string[] = [];

  ngOnInit() {
    this.selectedCategories = this.data?.selectedCategories ?? [];
    this.selectedTypes = this.data?.selectedTypes ?? [];
    this.productService.getCategories();
  }

  applyFilters() {
    this.dialogRef.close({
      selectedCategories: this.selectedCategories,
      selectedTypes: this.selectedTypes
    });
  }

  close(): void {
    this.dialogRef.close();
  }
}
