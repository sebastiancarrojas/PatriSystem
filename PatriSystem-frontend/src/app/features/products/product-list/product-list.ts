import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProductService } from '../../../core/services/product.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Product } from '../../../core/models/product.model';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './product-list.html',
  styleUrl: './product-list.scss'
})
export class ProductListComponent implements OnInit {
  private productService = inject(ProductService);
  private notification = inject(NotificationService);

  products = signal<Product[]>([]);
  loading = signal(false);
  displayedColumns: string[] = ['productName', 'barcode', 'categoryName', 'brandName', 'unitPrice', 'status', 'actions'];

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading.set(true);
    this.productService.getAll().subscribe({
      next: (data) => {
        this.products.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.notification.error('Error al cargar los productos');
        this.loading.set(false);
      }
    });
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