import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BaseChartDirective } from 'ng2-charts';
import { Chart, ChartData, ChartOptions, registerables } from 'chart.js';
import { DashboardService } from '../../../core/services/dashboard.service';
import { Dashboard } from '../../../core/models/dashboard.model';
import { NotificationService } from '../../../core/services/notification.service';


Chart.register(...registerables);

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    BaseChartDirective,
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class DashboardComponent implements OnInit {
  private dashboardService = inject(DashboardService);
  private notification = inject(NotificationService);
  private router = inject(Router);

  dashboard = signal<Dashboard | null>(null);
  loading = signal(false);

  chartData: ChartData<'line'> = { labels: [], datasets: [] };
  chartOptions: ChartOptions<'line'> = {
    responsive: true,
    plugins: { legend: { display: false } },
    scales: {
      x: { grid: { display: false } },
      y: {
        grid: { color: 'rgba(0,0,0,0.05)' },
        ticks: {
          callback: (value) => '$' + Number(value).toLocaleString('es-CO')
        }
      }
    }
  };

  ngOnInit(): void {
    this.loading.set(true);
    this.dashboardService.get().subscribe({
      next: (data) => {
        this.dashboard.set(data);
        this.buildChart(data);
        this.loading.set(false);
      },
      error: () => {
        this.notification.error('Error al cargar el dashboard');
        this.loading.set(false);
      }
    });
  }

  buildChart(data: Dashboard): void {
    this.chartData = {
      labels: data.last7DaysSales.map(d =>
        new Date(d.date).toLocaleDateString('es-CO', { day: '2-digit', month: 'short' })
      ),
      datasets: [{
        data: data.last7DaysSales.map(d => d.amount),
        borderColor: '#1976d2',
        backgroundColor: 'rgba(25,118,210,0.08)',
        borderWidth: 2,
        pointBackgroundColor: '#1976d2',
        pointRadius: 4,
        fill: true,
        tension: 0.4
      }]
    };
  }

  goToNewSale(): void {
  this.router.navigate(['/sales/create'], { queryParams: { returnUrl: '/dashboard' } });
  }

goToNewProduct(): void {
  this.router.navigate(['/products/create'], { queryParams: { returnUrl: '/dashboard' } });
  }
}