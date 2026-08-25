import { Component, inject, ChangeDetectionStrategy, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { A11yModule } from '@angular/cdk/a11y';

@Component({
  selector: 'app-sale-confirm-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, A11yModule],
  templateUrl: './sale-confirm-dialog.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styleUrl: './sale-confirm-dialog.scss',
})
export class SaleConfirmDialogComponent {
  private dialogRef = inject(MatDialogRef<SaleConfirmDialogComponent>);
  data = inject(MAT_DIALOG_DATA);

  @HostListener('keydown.enter')
  onEnter(): void {
    this.confirm();
  }

  confirm(): void {
    this.dialogRef.close(true);
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}