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
    path: 'categories',
    loadComponent: () =>
      import('./features/categories/category-list/category-list').then(
        (m) => m.CategoryListComponent
      )
  },
  {
    path: 'brands',
    loadComponent: () =>
      import('./features/brands/brand-list/brand-list').then(
        (m) => m.BrandListComponent
      )
  },
  {
    path: '',
    redirectTo: 'products',
    pathMatch: 'full'
  }
];