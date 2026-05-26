import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ApiService } from './core/services/api.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: `
    <h1>PatriSystem</h1>
    <p>Conexión a la API: {{ connectionStatus }}</p>
    <router-outlet />
  `
})
export class App implements OnInit {
  private apiService = inject(ApiService);
  private cdr = inject(ChangeDetectorRef);
  connectionStatus = 'Verificando...';

  ngOnInit(): void {
    this.apiService.get<any>('Products').subscribe({
      next: () => {
        this.connectionStatus = '✔ Conectado exitosamente';
        this.cdr.detectChanges();
      },
      error: () => {
        this.connectionStatus = '✘ Error de conexión';
        this.cdr.detectChanges();
      }
    });
  }
}