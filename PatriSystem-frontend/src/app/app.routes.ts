import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'products',
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
    path: '',
    redirectTo: 'products',
    pathMatch: 'full'
  }
];