import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { ApiResponse } from '../models/api-response.model';
import { Product, CreateProductRequest, UpdateProductRequest, ProductSearch } from '../models/product.model';
import { PaginationResponse, ProductPaginationRequest } from '../models/pagination.model';


@Injectable({ providedIn: 'root' })
export class ProductService {
  private api = inject(ApiService);

  getAll(): Observable<Product[]> {
    return this.api.get<Product[]>('Products');
  }

  getPaginated(request: ProductPaginationRequest): Observable<PaginationResponse<Product>> {
    return this.api.get<PaginationResponse<Product>>('Products/paginated', request as Record<string, any>);
  }

  getById(id: string): Observable<Product> {
    return this.api.get<Product>(`Products/${id}`);
  }

  create(product: CreateProductRequest): Observable<ApiResponse<object>> {
    return this.api.post<ApiResponse<object>>('Products', product);
  }

  update(id: string, product: UpdateProductRequest): Observable<ApiResponse<object>> {
    return this.api.put<ApiResponse<object>>(`Products/${id}`, product);
  }

  deactivate(id: string): Observable<ApiResponse<object>> {
    return this.api.delete<ApiResponse<object>>(`Products/${id}/deactivate`);
  }

  searchForSale(term: string): Observable<ProductSearch[]> {
  return this.api.get<ProductSearch[]>('Products/search', { term });
  }

  activate(id: string): Observable<ApiResponse<object>> {
  return this.api.patch<ApiResponse<object>>(`Products/${id}/activate`, {});
  }
}