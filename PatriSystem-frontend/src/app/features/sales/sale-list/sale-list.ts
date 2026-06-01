import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { SaleService} from '../../../core/services/sale.service';
import { SalePaginationRequest } from '../../../core/models/pagination.model';
import { NotificationService } from '../../../core/services/notification.service';
import { Sale } from '../../../core/models/sale.model';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { PaginatorComponent } from '../../../shared/components/paginator/paginator';

@Component({
  selector: 'app-sale-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    PaginatorComponent
  ],
  templateUrl: './sale-list.html',
  styleUrl: './sale-list.scss'
})
export class SaleListComponent implements OnInit {
  private saleService = inject(SaleService);
  private notification = inject(NotificationService);

  sales = signal<Sale[]>([]);
  loading = signal(false);
  currentPage = signal(1);
  totalPages = signal(1);
  displayedColumns: string[] = ['saleNumberFormatted', 'saleDate', 'totalAmount', 'items', 'actions'];

  searchControl = new FormControl('');
  startDateControl = new FormControl<Date | null>(null);
  endDateControl = new FormControl<Date | null>(null);

  request: SalePaginationRequest = {
    page: 1,
    recordsPerPage: 10
  };

  ngOnInit(): void {
    this.loadSales();

    this.searchControl.valueChanges.pipe(
      debounceTime(400),
      distinctUntilChanged()
    ).subscribe(value => {
      this.request.filter = value ?? undefined;
      this.request.page = 1;
      this.loadSales();
    });

    this.startDateControl.valueChanges.subscribe(value => {
      this.request.startDate = value?.toISOString() ?? undefined;
      this.request.page = 1;
      this.loadSales();
    });

    this.endDateControl.valueChanges.subscribe(value => {
      this.request.endDate = value?.toISOString() ?? undefined;
      this.request.page = 1;
      this.loadSales();
    });
  }

  loadSales(): void {
    this.loading.set(true);
    this.saleService.getPaginated(this.request).subscribe({
      next: (response) => {
        this.sales.set(response.items);
        this.totalPages.set(response.totalPages);
        this.currentPage.set(response.currentPage);
        this.loading.set(false);
      },
      error: () => {
        this.notification.error('Error al cargar las ventas');
        this.loading.set(false);
      }
    });
  }

  onPageChange(page: number): void {
    this.request.page = page;
    this.loadSales();
  }

  clearFilters(): void {
    this.searchControl.setValue('');
    this.startDateControl.setValue(null);
    this.endDateControl.setValue(null);
    this.request = { page: 1, recordsPerPage: 10 };
    this.loadSales();
  }
}