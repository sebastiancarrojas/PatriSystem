import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SaleService } from '../../../core/services/sale.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Sale } from '../../../core/models/sale.model';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-sale-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './sale-list.html',
  styleUrl: './sale-list.scss'
})
export class SaleListComponent implements OnInit {
  private saleService = inject(SaleService);
  private notification = inject(NotificationService);

  sales = signal<Sale[]>([]);
  loading = signal(false);
  displayedColumns: string[] = ['saleDate', 'totalAmount', 'items', 'actions'];

  ngOnInit(): void {
    this.loadSales();
  }

  loadSales(): void {
    this.loading.set(true);
    this.saleService.getAll().subscribe({
      next: (sales) => {
        this.sales.set(sales);
        this.loading.set(false);
      },
      error: () => {
        this.notification.error('Error al cargar las ventas');
        this.loading.set(false);
      }
    });
  }
}