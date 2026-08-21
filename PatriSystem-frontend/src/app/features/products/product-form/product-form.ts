import {
  Component,
  inject,
  OnInit,
  AfterViewInit,
  OnDestroy,
  signal,
  ViewChild,
  ElementRef,
  ChangeDetectionStrategy,
  LOCALE_ID,
} from '@angular/core';
import { CommonModule, registerLocaleData } from '@angular/common';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSelect, MatSelectModule } from '@angular/material/select';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialogModule } from '@angular/material/dialog';
import { A11yModule } from '@angular/cdk/a11y';
import localeEsCo from '@angular/common/locales/es-CO';

import { ProductService } from '../../../core/services/product.service';
import { NotificationService } from '../../../core/services/notification.service';
import { CategoryService } from '../../../core/services/category.service';
import { BrandService } from '../../../core/services/brand.service';
import { UnitOfMeasureService } from '../../../core/services/unit-of-measure.service';
import { Category } from '../../../core/models/category.model';
import { Brand } from '../../../core/models/brand.model';
import { UnitOfMeasure } from '../../../core/models/unit-of-measure.model';
import { CategoryDialogComponent } from '../../../shared/components/category-dialog/category-dialog';
import { BrandDialogComponent } from '../../../shared/components/brand-dialog/brand-dialog';
import { UnitOfMeasureDialogComponent } from '../../../shared/components/unit-of-measure-dialog/unit-of-measure-dialog';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog';

registerLocaleData(localeEsCo);

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    NgxMatSelectSearchModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    A11yModule,
  ],
  providers: [{ provide: LOCALE_ID, useValue: 'es-CO' }],
  templateUrl: './product-form.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styleUrl: './product-form.scss',
})
export class ProductFormComponent implements OnInit, AfterViewInit, OnDestroy {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  private brandService = inject(BrandService);
  private unitOfMeasureService = inject(UnitOfMeasureService);
  private notification = inject(NotificationService);
  private dialog = inject(MatDialog);

  @ViewChild('nameInput') nameInput?: ElementRef<HTMLInputElement>;
  @ViewChild('skuInput') skuInput?: ElementRef<HTMLInputElement>;
  @ViewChild('barcodeInput') barcodeInput?: ElementRef<HTMLInputElement>;
  @ViewChild('descriptionInput') descriptionInput?: ElementRef<HTMLTextAreaElement>;
  @ViewChild('priceInput') priceInput?: ElementRef<HTMLInputElement>;
  @ViewChild('stockInput') stockInput?: ElementRef<HTMLInputElement>;
  @ViewChild('categorySelect') categorySelect?: MatSelect;
  @ViewChild('brandSelect') brandSelect?: MatSelect;
  @ViewChild('unitSelect') unitSelect?: MatSelect;
  @ViewChild('submitButton') submitButton?: ElementRef<HTMLButtonElement>;

  @ViewChild('categoryWrap') categoryWrap?: ElementRef<HTMLElement>;
  @ViewChild('brandWrap') brandWrap?: ElementRef<HTMLElement>;
  @ViewChild('unitWrap') unitWrap?: ElementRef<HTMLElement>;

  private enterListenerCleanups: Array<() => void> = [];

  categories = signal<Category[]>([]);
  brands = signal<Brand[]>([]);
  unitsOfMeasure = signal<UnitOfMeasure[]>([]);
  filteredCategories = signal<Category[]>([]);
  filteredBrands = signal<Brand[]>([]);
  filteredUnitsOfMeasure = signal<UnitOfMeasure[]>([]);
  loading = signal(false);
  isEdit = signal(false);
  submitting = signal(false);
  productId = signal<string | null>(null);

  categoryFilterCtrl = new FormControl('');
  brandFilterCtrl = new FormControl('');
  unitFilterCtrl = new FormControl('');

  form: FormGroup = this.fb.group({
    productName: ['', Validators.required],
    sku: ['', Validators.required],
    barcode: [null],
    productDescription: [null],
    categoryId: ['', Validators.required],
    brandId: ['', Validators.required],
    unitPrice: [null, [Validators.required, Validators.min(0.01)]],
    unitOfMeasureId: [null],
    stockMin: [0, [Validators.min(0)]],
  });

  ngOnInit(): void {
    this.loadCategories();
    this.loadBrands();
    this.loadUnitsOfMeasure();

    this.categoryFilterCtrl.valueChanges.subscribe((term) => this.filterCategories(term));
    this.brandFilterCtrl.valueChanges.subscribe((term) => this.filterBrands(term));
    this.unitFilterCtrl.valueChanges.subscribe((term) => this.filterUnits(term));

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit.set(true);
      this.productId.set(id);
      this.loadProduct(id);
    } else {
      this.loadNextSku();
    }
  }

  ngAfterViewInit(): void {
    this.interceptEnter(this.categoryWrap, () => this.categorySelect, () => this.brandSelect?.focus());
    this.interceptEnter(this.brandWrap, () => this.brandSelect, () => this.unitSelect?.focus());
    this.interceptEnter(this.unitWrap, () => this.unitSelect, undefined);
    this.focusNameInput();
  }

  ngOnDestroy(): void {
    this.enterListenerCleanups.forEach((cleanup) => cleanup());
    this.enterListenerCleanups = [];
  }

  // Handles Enter inside a mat-select wrapper. Uses a native, capture-phase
  // listener because ngx-mat-select-search renders its input in a CDK overlay
  // outside this component's DOM, so a template (keydown.enter) binding on
  // the select itself won't see Enter presses made while typing a filter.
  private interceptEnter(
    wrap: ElementRef<HTMLElement> | undefined,
    getSelect: () => MatSelect | undefined,
    goNext?: () => void
  ): void {
    const el = wrap?.nativeElement;
    if (!el) return;

    const handler = (event: KeyboardEvent) => {
      if (event.key !== 'Enter') return;
      if (getSelect()?.panelOpen) return;
      event.preventDefault();
      event.stopImmediatePropagation();
      if (goNext) {
        setTimeout(() => goNext());
      }
    };

    el.addEventListener('keydown', handler, true);
    this.enterListenerCleanups.push(() => el.removeEventListener('keydown', handler, true));
  }

  private focusNameInput(): void {
    // Deferred to the next tick so the view is fully rendered before focusing.
    setTimeout(() => this.nameInput?.nativeElement.focus());
  }

  onFormKeydown(event: KeyboardEvent): void {
    if ((event.ctrlKey || event.metaKey) && event.key === 'Enter') {
      event.preventDefault();
      this.submit();
      return;
    }

    if (event.key === 'Escape') {
      event.preventDefault();
      event.stopPropagation();

      if (!this.form.dirty) {
        this.goBack();
        return;
      }

      const dialogRef = this.dialog.open<ConfirmDialogComponent, { message: string }, boolean>(
        ConfirmDialogComponent,
        {
          width: '360px',
          data: {
            message: '¿Estás seguro de que quieres salir? Los cambios que hiciste no se guardarán.',
          },
        }
      );

      dialogRef.afterClosed().subscribe((confirmed) => {
        if (confirmed) {
          this.goBack();
        }
      });
      return;
    }

    // Blocks the browser's implicit submit-on-Enter when a single submit
    // button exists in the form. Field-to-field navigation is already
    // handled by the individual onXEnter handlers above.
    if (event.key === 'Enter' && document.activeElement !== this.submitButton?.nativeElement) {
      event.preventDefault();
    }
  }

  onNameKeydown(event: KeyboardEvent): void {
    if (event.key === 'Tab' && event.shiftKey) {
      event.preventDefault();
    }
  }

  onNameEnter(event: Event): void {
    event.preventDefault();
    this.skuInput?.nativeElement.focus();
  }

  onSkuEnter(event: Event): void {
    event.preventDefault();
    this.barcodeInput?.nativeElement.focus();
  }

  onBarcodeEnter(event: Event): void {
    event.preventDefault();
    this.descriptionInput?.nativeElement.focus();
  }

  onDescriptionEnter(event: KeyboardEvent): void {
    if (event.key !== 'Enter' || event.isComposing) return;
    if (event.shiftKey) return;
    event.preventDefault();
    this.priceInput?.nativeElement.focus();
  }

  onPriceEnter(event: Event): void {
    event.preventDefault();
    this.stockInput?.nativeElement.focus();
  }

  onStockEnter(event: Event): void {
    event.preventDefault();
    this.categorySelect?.focus();
  }

  onCategorySelected(): void {
    this.brandSelect?.focus();
  }

  onBrandSelected(): void {
    this.unitSelect?.focus();
  }

  loadCategories(): void {
    this.categoryService.getAll().subscribe({
      next: (categories) => {
        this.categories.set(categories);
        this.filteredCategories.set(categories);
      },
      error: () => this.notification.error('Error al cargar las categorías'),
    });
  }

  loadBrands(): void {
    this.brandService.getAll().subscribe({
      next: (brands) => {
        this.brands.set(brands);
        this.filteredBrands.set(brands);
      },
      error: () => this.notification.error('Error al cargar las marcas'),
    });
  }

  loadUnitsOfMeasure(): void {
    this.unitOfMeasureService.getAll().subscribe({
      next: (unitsOfMeasure) => {
        this.unitsOfMeasure.set(unitsOfMeasure);
        this.filteredUnitsOfMeasure.set(unitsOfMeasure);
      },
      error: () => this.notification.error('Error al cargar las unidades de medida'),
    });
  }

  private filterCategories(term: string | null): void {
    const value = (term ?? '').toLowerCase();
    this.filteredCategories.set(
      this.categories().filter((c) => c.categoryName.toLowerCase().includes(value))
    );
  }

  private filterBrands(term: string | null): void {
    const value = (term ?? '').toLowerCase();
    this.filteredBrands.set(
      this.brands().filter((b) => b.brandName.toLowerCase().includes(value))
    );
  }

  private filterUnits(term: string | null): void {
    const value = (term ?? '').toLowerCase();
    this.filteredUnitsOfMeasure.set(
      this.unitsOfMeasure().filter((u) => u.name.toLowerCase().includes(value))
    );
  }

  loadNextSku(): void {
    this.productService.getNextSku().subscribe({
      next: (response) => {
        if (response.isSuccess && response.result) {
          this.form.controls['sku'].setValue(response.result);
        }
      },
      error: () => this.notification.error('Error al generar el Sku sugerido'),
    });
  }

  get selectedCategoryName(): string {
    const id = this.form.get('categoryId')?.value;
    return this.categories().find((c) => c.id === id)?.categoryName ?? '';
  }

  get selectedBrandName(): string {
    const id = this.form.get('brandId')?.value;
    return this.brands().find((b) => b.id === id)?.brandName ?? '';
  }

  get selectedUnitName(): string {
    const id = this.form.get('unitOfMeasureId')?.value;
    return this.unitsOfMeasure().find((u) => u.id === id)?.name ?? '';
  }

  loadProduct(id: string): void {
    this.loading.set(true);
    this.productService.getById(id).subscribe({
      next: (product) => {
        this.form.patchValue(product);
        this.loading.set(false);
        this.focusNameInput();
      },
      error: () => {
        this.notification.error('Error al cargar el producto');
        this.loading.set(false);
      },
    });
  }

  openCategoryDialog(): void {
    const dialogRef = this.dialog.open(CategoryDialogComponent, {
      width: '400px',
    });

    dialogRef.afterClosed().subscribe((category: Category | null) => {
      if (category) {
        const updated = [...this.categories(), category];
        this.categories.set(updated);
        this.filteredCategories.set(updated);
        this.form.controls['categoryId'].setValue(category.id);
      }
    });
  }

  openBrandDialog(): void {
    const dialogRef = this.dialog.open(BrandDialogComponent, {
      width: '400px',
    });

    dialogRef.afterClosed().subscribe((brand: Brand | null) => {
      if (brand) {
        const updated = [...this.brands(), brand];
        this.brands.set(updated);
        this.filteredBrands.set(updated);
        this.form.controls['brandId'].setValue(brand.id);
      }
    });
  }

  openUnitOfMeasureDialog(): void {
    const dialogRef = this.dialog.open(UnitOfMeasureDialogComponent, {
      width: '400px',
    });

    dialogRef.afterClosed().subscribe((unitOfMeasure: UnitOfMeasure | null) => {
      if (unitOfMeasure) {
        const updated = [...this.unitsOfMeasure(), unitOfMeasure];
        this.unitsOfMeasure.set(updated);
        this.filteredUnitsOfMeasure.set(updated);
        this.form.controls['unitOfMeasureId'].setValue(unitOfMeasure.id);
      }
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    this.submitting.set(true);
    const value = this.form.value;
    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') ?? '/products';

    if (this.isEdit() && this.productId()) {
      this.productService.update(this.productId()!, value).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.notification.success('Producto actualizado correctamente');
            this.router.navigate([returnUrl]);
          } else {
            this.notification.error(response.message);
          }
          this.submitting.set(false);
        },
        error: () => {
          this.notification.error('Error al actualizar el producto');
          this.submitting.set(false);
        },
      });
    } else {
      this.productService.create(value).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.notification.success('Producto creado correctamente');
            this.router.navigate([returnUrl]);
          } else {
            this.notification.error(response.message);
          }
          this.submitting.set(false);
        },
        error: () => {
          this.notification.error('Error al crear el producto');
          this.submitting.set(false);
        },
      });
    }
  }

  goBack(): void {
    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') ?? '/products';
    this.router.navigate([returnUrl]);
  }
}