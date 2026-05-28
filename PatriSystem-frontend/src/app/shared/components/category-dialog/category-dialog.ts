import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CategoryService } from '../../../core/services/category.service';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-category-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './category-dialog.html',
  styleUrl: './category-dialog.scss'
})
export class CategoryDialogComponent {
  private fb = inject(FormBuilder);
  private categoryService = inject(CategoryService);
  private notification = inject(NotificationService);
  private dialogRef = inject(MatDialogRef<CategoryDialogComponent>);

  form = this.fb.group({
    categoryName: ['', Validators.required]
  });

  submit(): void {
    if (this.form.invalid) return;

    this.categoryService.create(this.form.value as any).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.notification.success('Categoría creada correctamente');
          this.dialogRef.close(true);
        } else {
          this.notification.error(response.message);
        }
      },
      error: () => this.notification.error('Error al crear la categoría')
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}