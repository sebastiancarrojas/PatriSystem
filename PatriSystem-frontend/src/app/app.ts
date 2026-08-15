import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { filter, map } from 'rxjs';
import { NavbarComponent } from './layout/navbar/navbar';
import { routeAnimations } from './animations';

const NO_NAVBAR_ROUTES = ['/login', '/forgot-password', '/reset-password'];

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent],
  changeDetection: ChangeDetectionStrategy.Eager,
  animations: [routeAnimations],
  template: `
    @if (showNavbar()) {
      <app-navbar />
    }
    <div class="route-container" [@routeAnimations]="outlet.isActivated ? outlet.activatedRoute : ''">
      <router-outlet #outlet="outlet" />
    </div>
  `,
  styles: [`
    .route-container {
      position: relative;
      overflow: hidden;
    }
  `],
})
export class App {
  private router = inject(Router);

  showNavbar = toSignal(
    this.router.events.pipe(
      filter((event): event is NavigationEnd => event instanceof NavigationEnd),
      map((event) => !NO_NAVBAR_ROUTES.some((route) => event.urlAfterRedirects.startsWith(route)))
    ),
    {
      initialValue: !NO_NAVBAR_ROUTES.some((route) => window.location.pathname.startsWith(route)),
    }
  );
}