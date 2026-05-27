import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { BrandService } from '../../../core/services/brand.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Brand } from '../../../core/models/brand.model';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

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
    MatProgressSpinnerModule
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
  displayedColumns: string[] = ['brandName', 'brandDescription', 'createdAt'];

  form = this.fb.group({
    brandName: ['', Validators.required],
    brandDescription: [null]
  });

  ngOnInit(): void {
    this.loadBrands();
  }

  loadBrands(): void {
    this.loading.set(true);
    this.brandService.getAll().subscribe({
      next: (brands) => {
        this.brands.set(brands);
        this.loading.set(false);
      },
      error: () => {
        this.notification.error('Error al cargar las marcas');
        this.loading.set(false);
      }
    });
  }

  submit(): void {
    if (this.form.invalid) return;
    this.brandService.create(this.form.value as any).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.notification.success('Marca creada correctamente');
          this.form.reset();
          this.showForm.set(false);
          this.loadBrands();
        } else {
          this.notification.error(response.message);
        }
      },
      error: () => this.notification.error('Error al crear la marca')
    });
  }
}