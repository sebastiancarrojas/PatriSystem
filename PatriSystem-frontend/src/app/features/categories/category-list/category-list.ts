import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { CategoryService } from '../../../core/services/category.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Category } from '../../../core/models/category.model';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-category-list',
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
  templateUrl: './category-list.html',
  styleUrl: './category-list.scss'
})
export class CategoryListComponent implements OnInit {
  private categoryService = inject(CategoryService);
  private notification = inject(NotificationService);
  private fb = inject(FormBuilder);

  categories = signal<Category[]>([]);
  loading = signal(false);
  showForm = signal(false);
  displayedColumns: string[] = ['categoryName', 'createdAt'];

  form = this.fb.group({
    categoryName: ['', Validators.required]
  });

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.loading.set(true);
    this.categoryService.getAll().subscribe({
      next: (categories) => {
        this.categories.set(categories);
        this.loading.set(false);
      },
      error: () => {
        this.notification.error('Error al cargar las categorías');
        this.loading.set(false);
      }
    });
  }

  submit(): void {
    if (this.form.invalid) return;
    this.categoryService.create(this.form.value as any).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.notification.success('Categoría creada correctamente');
          this.form.reset();
          this.showForm.set(false);
          this.loadCategories();
        } else {
          this.notification.error(response.message);
        }
      },
      error: () => this.notification.error('Error al crear la categoría')
    });
  }
}