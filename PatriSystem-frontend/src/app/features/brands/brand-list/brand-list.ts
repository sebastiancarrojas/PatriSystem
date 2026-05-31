import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormControl, Validators } from '@angular/forms';
import { BrandService } from '../../../core/services/brand.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Brand } from '../../../core/models/brand.model';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { PaginatorComponent } from '../../../shared/components/paginator/paginator';
import { debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-brand-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressSpinnerModule,
    PaginatorComponent
  ],
  templateUrl: './brand-list.html',
  styleUrl: './brand-list.scss'
})
export class BrandListComponent implements OnInit {
  private brandService = inject(BrandService);
  private notification = inject(NotificationService);
  private fb = inject(FormBuilder);

  brands = signal<Brand[]>([]);
  loading = signal(false);
  showForm = signal(false);
  editingBrand = signal<Brand | null>(null);
  currentPage = signal(1);
  totalPages = signal(1);
  displayedColumns: string[] = ['brandName', 'brandDescription', 'createdAt', 'actions'];

  searchControl = new FormControl('');

  form = this.fb.group({
  brandName: ['', Validators.required],
  brandDescription: [null as string | null]
  });

  ngOnInit(): void {
    this.loadBrands();

    this.searchControl.valueChanges.pipe(
      debounceTime(400),
      distinctUntilChanged()
    ).subscribe(() => {
      this.currentPage.set(1);
      this.loadBrands();
    });
  }

  loadBrands(): void {
    this.loading.set(true);
    this.brandService.getPaginated(this.currentPage(), this.searchControl.value ?? undefined).subscribe({
      next: (response) => {
        this.brands.set(response.items);
        this.totalPages.set(response.totalPages);
        this.loading.set(false);
      },
      error: () => {
        this.notification.error('Error al cargar las marcas');
        this.loading.set(false);
      }
    });
  }

  onPageChange(page: number): void {
    this.currentPage.set(page);
    this.loadBrands();
  }

  openCreate(): void {
    this.editingBrand.set(null);
    this.form.reset();
    this.showForm.set(true);
  }

  openEdit(brand: Brand): void {
    this.editingBrand.set(brand);
    this.form.patchValue({ brandName: brand.brandName, brandDescription: brand.brandDescription });
    this.showForm.set(true);
  }

  submit(): void {
    if (this.form.invalid) return;

    const editing = this.editingBrand();
    if (editing) {
      this.brandService.update(editing.id, this.form.value.brandName!, this.form.value.brandDescription ?? undefined).subscribe({
        next: () => {
          this.notification.success('Marca actualizada correctamente');
          this.form.reset();
          this.showForm.set(false);
          this.editingBrand.set(null);
          this.loadBrands();
        },
        error: () => this.notification.error('Error al actualizar la marca')
      });
    } else {
      this.brandService.create(this.form.value as any).subscribe({
        next: () => {
          this.notification.success('Marca creada correctamente');
          this.form.reset();
          this.showForm.set(false);
          this.loadBrands();
        },
        error: () => this.notification.error('Error al crear la marca')
      });
    }
  }

  cancel(): void {
    this.showForm.set(false);
    this.editingBrand.set(null);
    this.form.reset();
  }
}