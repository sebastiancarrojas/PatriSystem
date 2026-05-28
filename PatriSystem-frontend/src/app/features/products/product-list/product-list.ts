import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { ProductService } from '../../../core/services/product.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Product } from '../../../core/models/product.model';
import { ProductPaginationRequest } from '../../../core/models/pagination.model';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { PaginatorComponent } from '../../../shared/components/paginator/paginator';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatInputModule,
    PaginatorComponent
  ],
  templateUrl: './product-list.html',
  styleUrl: './product-list.scss'
})
export class ProductListComponent implements OnInit {
  private productService = inject(ProductService);
  private notification = inject(NotificationService);

  products = signal<Product[]>([]);
  loading = signal(false);
  totalCount = signal(0);
  currentPage = signal(1);
  totalPages = signal(1);
  displayedColumns: string[] = ['productName', 'barcode', 'categoryName', 'brandName', 'unitPrice', 'status', 'actions'];

  searchControl = new FormControl('');

  request: ProductPaginationRequest = {
    page: 1,
    recordsPerPage: 10
  };

  ngOnInit(): void {
    this.loadProducts();

    this.searchControl.valueChanges.pipe(
      debounceTime(400),
      distinctUntilChanged()
    ).subscribe(value => {
      this.request.filter = value ?? undefined;
      this.request.page = 1;
      this.loadProducts();
    });
  }

  loadProducts(): void {
    this.loading.set(true);
    this.productService.getPaginated(this.request).subscribe({
      next: (response) => {
        this.products.set(response.items);
        this.totalCount.set(response.totalCount);
        this.currentPage.set(response.currentPage);
        this.totalPages.set(response.totalPages);
        this.loading.set(false);
      },
      error: () => {
        this.notification.error('Error al cargar los productos');
        this.loading.set(false);
      }
    });
  }

  onPageChange(page: number): void {
    this.request.page = page;
    this.loadProducts();
  }

  deactivate(id: string): void {
    this.productService.deactivate(id).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.notification.success('Producto desactivado correctamente');
          this.loadProducts();
        } else {
          this.notification.error(response.message);
        }
      },
      error: () => {
        this.notification.error('Error al desactivar el producto');
      }
    });
  }
}