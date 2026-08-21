import { Component, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AuthService } from '../../core/services/auth.service';

const COLLAPSED_STORAGE_KEY = 'patrisystem-navbar-collapsed';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterModule, MatButtonModule, MatIconModule, MatTooltipModule],
  templateUrl: './navbar.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './navbar.scss',
  host: {
    '[class.collapsed]': 'collapsed()',
  },
})
export class NavbarComponent {
  private authService = inject(AuthService);

  collapsed = signal(localStorage.getItem(COLLAPSED_STORAGE_KEY) === 'true');

  links = [
    { path: '/dashboard', icon: 'dashboard', label: 'Dashboard' },
    { path: '/products', icon: 'inventory_2', label: 'Productos' },
    { path: '/categories', icon: 'category', label: 'Categorías' },
    { path: '/brands', icon: 'branding_watermark', label: 'Marcas' },
    { path: '/sales', icon: 'point_of_sale', label: 'Ventas' },
  ];

  toggleCollapsed(): void {
    this.collapsed.update((value) => !value);
    localStorage.setItem(COLLAPSED_STORAGE_KEY, String(this.collapsed()));
  }

  logout(): void {
    this.authService.logout();
  }

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }
}