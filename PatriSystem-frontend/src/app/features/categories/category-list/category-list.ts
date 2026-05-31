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
import { PaginatorComponent } from '../../../shared/components/paginator/paginator';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';

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
    MatProgressSpinnerModule,
    PaginatorComponent
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
  editingCategory = signal<Category | null>(null);
  currentPage = signal(1);
  totalPages = signal(1);
  displayedColumns: string[] = ['categoryName', 'createdAt', 'actions'];

  searchControl = new FormControl('');

  form = this.fb.group({
    categoryName: ['', Validators.required]
  });

  ngOnInit(): void {
    this.loadCategories();

    this.searchControl.valueChanges.pipe(
      debounceTime(400),
      distinctUntilChanged()
    ).subscribe(() => {
      this.currentPage.set(1);
      this.loadCategories();
    });
  }

  loadCategories(): void {
    this.loading.set(true);
    this.categoryService.getPaginated(this.currentPage(), this.searchControl.value ?? undefined).subscribe({
      next: (response) => {
        this.categories.set(response.items);
        this.totalPages.set(response.totalPages);
        this.loading.set(false);
      },
      error: () => {
        this.notification.error('Error al cargar las categorías');
        this.loading.set(false);
      }
    });
  }

  onPageChange(page: number): void {
    this.currentPage.set(page);
    this.loadCategories();
  }

  openCreate(): void {
    this.editingCategory.set(null);
    this.form.reset();
    this.showForm.set(true);
  }

  openEdit(category: Category): void {
    this.editingCategory.set(category);
    this.form.patchValue({ categoryName: category.categoryName });
    this.showForm.set(true);
  }

  submit(): void {
    if (this.form.invalid) return;

    const editing = this.editingCategory();
    if (editing) {
      this.categoryService.update(editing.id, this.form.value as any).subscribe({
        next: () => {
          this.notification.success('Categoría actualizada correctamente');
          this.form.reset();
          this.showForm.set(false);
          this.editingCategory.set(null);
          this.loadCategories();
        },
        error: () => this.notification.error('Error al actualizar la categoría')
      });
    } else {
      this.categoryService.create(this.form.value as any).subscribe({
        next: () => {
          this.notification.success('Categoría creada correctamente');
          this.form.reset();
          this.showForm.set(false);
          this.loadCategories();
        },
        error: () => this.notification.error('Error al crear la categoría')
      });
    }
  }

  cancel(): void {
    this.showForm.set(false);
    this.editingCategory.set(null);
    this.form.reset();
  }
}