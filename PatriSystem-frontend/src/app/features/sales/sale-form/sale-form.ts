import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { SaleService } from '../../../core/services/sale.service';
import { ProductService } from '../../../core/services/product.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Product } from '../../../core/models/product.model';
import { CreateSaleDetailRequest } from '../../../core/models/sale.model';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-sale-form',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './sale-form.html',
  styleUrl: './sale-form.scss'
})
export class SaleFormComponent implements OnInit {
  private saleService = inject(SaleService);
  private productService = inject(ProductService);
  private notification = inject(NotificationService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  products = signal<Product[]>([]);
  details = signal<(CreateSaleDetailRequest & { productName: string; unitPrice: number; subTotal: number })[]>([]);
  loading = signal(false);
  displayedColumns: string[] = ['productName', 'quantity', 'unitPrice', 'subTotal', 'actions'];

  form = this.fb.group({
    productId: ['', Validators.required],
    quantity: [1, [Validators.required, Validators.min(1)]]
  });

  get total(): number {
    return this.details().reduce((acc, d) => acc + d.subTotal, 0);
  }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getAll().subscribe({
      next: (products) => this.products.set(products.filter(p => p.status)),
      error: () => this.notification.error('Error al cargar los productos')
    });
  }

  addDetail(): void {
    if (this.form.invalid) return;

    const productId = this.form.value.productId!;
    const quantity = this.form.value.quantity!;
    const product = this.products().find(p => p.id === productId);

    if (!product) return;

    const existing = this.details().find(d => d.productId === productId);
    if (existing) {
      this.notification.error('El producto ya está en la venta');
      return;
    }

    const subTotal = product.unitPrice * quantity;

    this.details.update(details => [...details, {
      productId,
      quantity,
      productName: product.productName,
      unitPrice: product.unitPrice,
      subTotal
    }]);

    this.form.reset({ productId: '', quantity: 1 });
  }

  removeDetail(productId: string): void {
    this.details.update(details => details.filter(d => d.productId !== productId));
  }

  submit(): void {
    if (this.details().length === 0) {
      this.notification.error('Agrega al menos un producto');
      return;
    }

    this.loading.set(true);
    const sale = {
      details: this.details().map(d => ({
        productId: d.productId,
        quantity: d.quantity
      }))
    };

    this.saleService.create(sale).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.notification.success('Venta registrada correctamente');
          this.router.navigate(['/sales']);
        } else {
          this.notification.error(response.message);
        }
        this.loading.set(false);
      },
      error: () => {
        this.notification.error('Error al registrar la venta');
        this.loading.set(false);
      }
    });
  }
}