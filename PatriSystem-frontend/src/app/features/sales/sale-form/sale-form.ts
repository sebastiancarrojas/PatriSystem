import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormControl, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged, switchMap, of } from 'rxjs';
import { SaleService } from '../../../core/services/sale.service';
import { ProductService } from '../../../core/services/product.service';
import { NotificationService } from '../../../core/services/notification.service';
import { ProductSearch } from '../../../core/models/product.model';
import { CreateSaleDetailRequest } from '../../../core/models/sale.model';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
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
    MatAutocompleteModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './sale-form.html',
  styleUrl: './sale-form.scss'
})
export class SaleFormComponent {
  private saleService = inject(SaleService);
  private productService = inject(ProductService);
  private notification = inject(NotificationService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  searchResults = signal<ProductSearch[]>([]);
  details = signal<(CreateSaleDetailRequest & { productName: string; unitPrice: number; subTotal: number })[]>([]);
  loading = signal(false);
  displayedColumns: string[] = ['productName', 'quantity', 'unitPrice', 'subTotal', 'actions'];

  searchControl = new FormControl('');
  quantityControl = new FormControl(1, [Validators.required, Validators.min(1)]);

  selectedProduct = signal<ProductSearch | null>(null);

  get total(): number {
    return this.details().reduce((acc, d) => acc + d.subTotal, 0);
  }

  constructor() {
    this.searchControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(term => {
        if (!term || term.length < 2) return of([]);
        return this.productService.searchForSale(term);
      })
    ).subscribe(results => this.searchResults.set(results));
  }

  onProductSelected(product: ProductSearch): void {
    this.selectedProduct.set(product);
    this.searchControl.setValue(product.productName, { emitEvent: false });
  }

  displayProduct(product: ProductSearch): string {
    return product ? product.productName : '';
  }

  addDetail(): void {
    const product = this.selectedProduct();
    const quantity = this.quantityControl.value!;

    if (!product) {
      this.notification.error('Selecciona un producto');
      return;
    }

    if (quantity <= 0) {
      this.notification.error('La cantidad debe ser mayor a 0');
      return;
    }

    const existing = this.details().find(d => d.productId === product.id);
    if (existing) {
      this.details.update(details =>
        details.map(d => d.productId === product.id
          ? { ...d, quantity: d.quantity + quantity, subTotal: d.unitPrice * (d.quantity + quantity) }
          : d
        )
      );
    } else {
      const subTotal = product.unitPrice * quantity;
      this.details.update(details => [...details, {
        productId: product.id,
        quantity,
        productName: product.productName,
        unitPrice: product.unitPrice,
        subTotal
      }]);
    }

    this.searchControl.setValue('');
    this.quantityControl.setValue(1);
    this.selectedProduct.set(null);
    this.searchResults.set([]);
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