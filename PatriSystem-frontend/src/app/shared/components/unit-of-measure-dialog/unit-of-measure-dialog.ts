import { Component, inject, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { UnitOfMeasureService } from '../../../core/services/unit-of-measure.service';
import { NotificationService } from '../../../core/services/notification.service';
import { UnitOfMeasure } from '../../../core/models/unit-of-measure.model';

@Component({
  selector: 'app-unit-of-measure-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
  templateUrl: './unit-of-measure-dialog.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './unit-of-measure-dialog.scss',
})
export class UnitOfMeasureDialogComponent {
  private fb = inject(FormBuilder);
  private unitOfMeasureService = inject(UnitOfMeasureService);
  private notification = inject(NotificationService);
  private dialogRef = inject(MatDialogRef<UnitOfMeasureDialogComponent>);

  form = this.fb.group({
    name: ['', Validators.required],
  });

  submit(): void {
    if (this.form.invalid) return;

    this.unitOfMeasureService.create(this.form.value as any).subscribe({
      next: (unitOfMeasure: UnitOfMeasure) => {
        this.notification.success('Unidad de medida creada correctamente');
        this.dialogRef.close(unitOfMeasure);
      },
      error: () => this.notification.error('Error al crear la unidad de medida'),
    });
  }

  cancel(): void {
    this.dialogRef.close(null);
  }
}