import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login.component').then(
        (m) => m.LoginComponent
      )
  },
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/dashboard/dashboard/dashboard').then(
        (m) => m.DashboardComponent
      )
  },
  {
    path: 'products',
    canActivate: [authGuard],
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./features/products/product-list/product-list').then(
            (m) => m.ProductListComponent
          )
      },
      {
        path: 'create',
        loadComponent: () =>
          import('./features/products/product-form/product-form').then(
            (m) => m.ProductFormComponent
          )
      },
      {
        path: 'edit/:id',
        loadComponent: () =>
          import('./features/products/product-form/product-form').then(
            (m) => m.ProductFormComponent
          )
      }
    ]
  },
  {
    path: 'categories',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/categories/category-list/category-list').then(
        (m) => m.CategoryListComponent
      )
  },
  {
    path: 'brands',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/brands/brand-list/brand-list').then(
        (m) => m.BrandListComponent
      )
  },
  {
    path: 'sales',
    canActivate: [authGuard],
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./features/sales/sale-list/sale-list').then(
            (m) => m.SaleListComponent
          )
      },
      {
        path: 'create',
        loadComponent: () =>
          import('./features/sales/sale-form/sale-form').then(
            (m) => m.SaleFormComponent
          )
      },
      {
        path: ':id',
        loadComponent: () =>
          import('./features/sales/sale-detail/sale-detail').then(
            (m) => m.SaleDetailComponent
          )
      }
    ]
  }
];