import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { SaleService } from '../../../core/services/sale.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Sale } from '../../../core/models/sale.model';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-sale-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatCardModule
  ],
  templateUrl: './sale-detail.html',
  styleUrl: './sale-detail.scss'
})
export class SaleDetailComponent implements OnInit {
  private saleService = inject(SaleService);
  private notification = inject(NotificationService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  sale = signal<Sale | null>(null);
  loading = signal(false);
  displayedColumns: string[] = ['productName', 'quantity', 'unitPrice', 'subTotal'];

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) this.loadSale(id);
  }

  loadSale(id: string): void {
    this.loading.set(true);
    this.saleService.getById(id).subscribe({
      next: (sale) => {
        this.sale.set(sale);
        this.loading.set(false);
      },
      error: () => {
        this.notification.error('Error al cargar la venta');
        this.loading.set(false);
      }
    });
  }

  goBack(): void {
    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') ?? '/sales';
    this.router.navigate([returnUrl]);
  }
}