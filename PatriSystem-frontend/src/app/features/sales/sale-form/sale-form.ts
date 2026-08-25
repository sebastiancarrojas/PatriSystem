import {
  Component,
  inject,
  signal,
  ChangeDetectionStrategy,
  ViewChild,
  ElementRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { ReactiveFormsModule, FormControl, Validators } from '@angular/forms';
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
import {
  MatAutocompleteModule,
  MatAutocompleteSelectedEvent,
} from '@angular/material/autocomplete';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { SaleConfirmDialogComponent } from '../../../shared/components/sale-confirm-dialog/sale-confirm-dialog';
import { TempProductDialogComponent } from '../../../shared/components/temp-product-dialog/temp-product-dialog';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog';
import { A11yModule } from '@angular/cdk/a11y';

type SaleDetailRow = CreateSaleDetailRequest & {
  productName: string;
  subTotal: number;
};

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
    MatProgressSpinnerModule,
    MatDialogModule,
  ],
  templateUrl: './sale-form.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styleUrl: './sale-form.scss',
})
export class SaleFormComponent {
  private saleService = inject(SaleService);
  private productService = inject(ProductService);
  private notification = inject(NotificationService);
  private router = inject(Router);
  private dialog = inject(MatDialog);
  private route = inject(ActivatedRoute);

  @ViewChild('searchInputRef') searchInputRef?: ElementRef<HTMLInputElement>;
  @ViewChild('quantityInputRef') quantityInputRef?: ElementRef<HTMLInputElement>;
  @ViewChild('searchInput') searchInput?: ElementRef;

  searchResults = signal<ProductSearch[]>([]);
  details = signal<SaleDetailRow[]>([]);
  loading = signal(false);
  displayedColumns: string[] = ['productName', 'quantity', 'unitPrice', 'subTotal', 'actions'];

  searchControl = new FormControl('');
  quantityControl = new FormControl(1, [Validators.required, Validators.min(1)]);

  selectedProduct = signal<ProductSearch | null>(null);

  get total(): number {
    return this.details().reduce((acc, d) => acc + d.subTotal, 0);
  }

  get totalItems(): number {
    return this.details().reduce((acc, d) => acc + d.quantity, 0);
  }

  constructor() {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        switchMap((term) => {
          if (!term || typeof term !== 'string' || term.length < 2) return of([]);
          return this.productService.searchForSale(term);
        }),
      )
      .subscribe((results) => this.searchResults.set(results));
  }

  onProductSelected(event: MatAutocompleteSelectedEvent): void {
    const product = event.option.value as ProductSearch;
    this.selectedProduct.set(product);
    this.searchControl.setValue(product.productName, { emitEvent: false });
    this.focusQuantity();
  }

  private focusQuantity(): void {
    setTimeout(() => {
      this.quantityInputRef?.nativeElement.focus({ preventScroll: true });
      this.quantityInputRef?.nativeElement.select();
    });
  }

  private focusSearch(): void {
    setTimeout(() => this.searchInputRef?.nativeElement.focus({ preventScroll: true }));
  }

  onQuantityEnter(): void {
    this.addDetail();
  }

  addDetail(): void {
    const product = this.selectedProduct();
    const quantity = this.quantityControl.value ?? 0;

    if (!product) {
      this.notification.error('Selecciona un producto de la lista');
      this.focusSearch();
      return;
    }

    if (!quantity || quantity <= 0) {
      this.notification.error('La cantidad debe ser mayor a 0');
      return;
    }

    const existing = this.details().find((d) => !d.isTemporary && d.productId === product.id);

    if (existing) {
      this.details.update((details) =>
        details.map((d) =>
          !d.isTemporary && d.productId === product.id
            ? {
                ...d,
                quantity: d.quantity + quantity,
                subTotal: d.unitPrice * (d.quantity + quantity),
              }
            : d,
        ),
      );
    } else {
      const subTotal = product.unitPrice * quantity;
      this.details.update((details) => [
        ...details,
        {
          productId: product.id,
          quantity,
          productName: product.productName,
          unitPrice: product.unitPrice,
          isTemporary: false,
          subTotal,
        },
      ]);
    }

    this.searchControl.setValue('');
    this.quantityControl.setValue(1);
    this.selectedProduct.set(null);
    this.searchResults.set([]);
    this.focusSearch();
  }

  removeDetail(detail: SaleDetailRow): void {
    this.details.update((details) => details.filter((d) => d !== detail));
  }

  isQuantityInvalid(detail: SaleDetailRow): boolean {
    return !detail.quantity || detail.quantity <= 0;
  }

  isPriceInvalid(detail: SaleDetailRow): boolean {
    return !detail.unitPrice || detail.unitPrice <= 0;
  }

  updateQuantity(detail: SaleDetailRow, quantity: number): void {
    if (!quantity || quantity <= 0) {
      this.notification.error('La cantidad debe ser mayor a 0');
      return;
    }
    this.details.update((details) =>
      details.map((d) => (d === detail ? { ...d, quantity, subTotal: d.unitPrice * quantity } : d)),
    );
  }

  updatePrice(detail: SaleDetailRow, price: number): void {
    if (!price || price <= 0) {
      this.notification.error('El precio debe ser mayor a 0');
      return;
    }
    this.details.update((details) =>
      details.map((d) => (d === detail ? { ...d, unitPrice: price, subTotal: price * d.quantity } : d)),
    );
  }

  openTempProductDialog(): void {
    const dialogRef = this.dialog.open(TempProductDialogComponent, { width: '400px' });

    dialogRef
      .afterClosed()
      .subscribe((product: { productName: string; unitPrice: number } | null) => {
        if (product) {
          this.details.update((details) => [
            ...details,
            {
              quantity: 1,
              productName: product.productName,
              unitPrice: product.unitPrice,
              isTemporary: true,
              subTotal: product.unitPrice,
            },
          ]);
        }
        this.focusSearch();
      });
  }

  submit(): void {
    if (this.loading()) return;

    if (this.details().length === 0) {
      this.notification.error('Agrega al menos un producto');
      return;
    }

    const dialogRef = this.dialog.open(SaleConfirmDialogComponent, {
      width: '400px',
      data: { total: this.total, items: this.details().length },
    });

    dialogRef.afterClosed().subscribe((confirmed) => {
      if (confirmed) this.registerSale();
    });
  }

  registerSale(): void {
    this.loading.set(true);
    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') ?? '/sales';
    const sale = {
      details: this.details().map((d) => ({
        productId: d.isTemporary ? undefined : d.productId,
        quantity: d.quantity,
        unitPrice: d.unitPrice,
        isTemporary: d.isTemporary,
        productName: d.isTemporary ? d.productName : undefined,
      })),
    };

    this.saleService.create(sale).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.notification.success('Venta registrada correctamente');
          this.router.navigate([returnUrl]);
        } else {
          this.notification.error(response.message);
        }
        this.loading.set(false);
      },
      error: () => {
        this.notification.error('Error al registrar la venta');
        this.loading.set(false);
      },
    });
  }

  goBack(): void {
    if (this.details().length === 0) {
      this.navigateBack();
      return;
    }

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: { message: 'Hay productos sin guardar en esta venta. ¿Deseas salir de todas formas?' },
    });

    dialogRef.afterClosed().subscribe((confirmed) => {
      if (confirmed) this.navigateBack();
    });
  }

  private navigateBack(): void {
    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') ?? '/sales';
    this.router.navigate([returnUrl]);
  }

  handleKeydown(event: KeyboardEvent): void {
    if ((event.ctrlKey || event.metaKey) && event.key === 'Enter') {
      event.preventDefault();
      this.submit();
      return;
    }
    if (event.key === 'Escape') {
      event.preventDefault();
      event.stopPropagation();
      this.goBack();
    }
  }

  ngAfterViewInit(): void {
    this.searchInputRef?.nativeElement.focus({ preventScroll: true });
  }

  onSearchKeydown(event: KeyboardEvent): void {
    if (event.shiftKey && event.key === 'Tab') {
      event.preventDefault();
    }
  }

}