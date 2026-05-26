import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductService } from '../../../core/services/product.service';
import { NotificationService } from '../../../core/services/notification.service';
import { CategoryService } from '../../../core/services/category.service';
import { BrandService } from '../../../core/services/brand.service';
import { Category } from '../../../core/models/category.model';
import { Brand } from '../../../core/models/brand.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

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
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './product-form.html',
  styleUrl: './product-form.scss'
})
export class ProductFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  private brandService = inject(BrandService);
  private notification = inject(NotificationService);

  categories = signal<Category[]>([]);
  brands = signal<Brand[]>([]);
  loading = signal(false);
  isEdit = signal(false);
  productId = signal<string | null>(null);

  form: FormGroup = this.fb.group({
    productName: ['', Validators.required],
    barcode: [null],
    productDescription: [null],
    categoryId: ['', Validators.required],
    brandId: ['', Validators.required],
    unitPrice: [null, [Validators.required, Validators.min(0.01)]],
    unitOfMeasure: [null]
  });

  ngOnInit(): void {
    this.loadCategories();
    this.loadBrands();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit.set(true);
      this.productId.set(id);
      this.loadProduct(id);
    }
  }

  loadCategories(): void {
    this.categoryService.getAll().subscribe({
      next: (categories) => this.categories.set(categories),
      error: () => this.notification.error('Error al cargar las categorías')
    });
  }

  loadBrands(): void {
    this.brandService.getAll().subscribe({
      next: (brands) => this.brands.set(brands),
      error: () => this.notification.error('Error al cargar las marcas')
    });
  }

  loadProduct(id: string): void {
    this.loading.set(true);
    this.productService.getById(id).subscribe({
      next: (product) => {
        this.form.patchValue(product);
        this.loading.set(false);
      },
      error: () => {
        this.notification.error('Error al cargar el producto');
        this.loading.set(false);
      }
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    this.loading.set(true);
    const value = this.form.value;

    if (this.isEdit() && this.productId()) {
      this.productService.update(this.productId()!, value).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.notification.success('Producto actualizado correctamente');
            this.router.navigate(['/products']);
          } else {
            this.notification.error(response.message);
          }
          this.loading.set(false);
        },
        error: () => {
          this.notification.error('Error al actualizar el producto');
          this.loading.set(false);
        }
      });
    } else {
      this.productService.create(value).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.notification.success('Producto creado correctamente');
            this.router.navigate(['/products']);
          } else {
            this.notification.error(response.message);
          }
          this.loading.set(false);
        },
        error: () => {
          this.notification.error('Error al crear el producto');
          this.loading.set(false);
        }
      });
    }
  }
}