import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { BrandService } from '../../../core/services/brand.service';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-brand-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './brand-dialog.html',
  styleUrl: './brand-dialog.scss'
})
export class BrandDialogComponent {
  private fb = inject(FormBuilder);
  private brandService = inject(BrandService);
  private notification = inject(NotificationService);
  private dialogRef = inject(MatDialogRef<BrandDialogComponent>);

  form = this.fb.group({
    brandName: ['', Validators.required],
    brandDescription: [null]
  });

  submit(): void {
    if (this.form.invalid) return;

    this.brandService.create(this.form.value as any).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.notification.success('Marca creada correctamente');
          this.dialogRef.close(true);
        } else {
          this.notification.error(response.message);
        }
      },
      error: () => this.notification.error('Error al crear la marca')
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}