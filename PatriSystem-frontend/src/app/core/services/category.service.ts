import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Category, CreateCategoryRequest } from '../models/category.model';
import { PaginationResponse } from '../models/pagination.model';

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private api = inject(ApiService);

  getAll(): Observable<Category[]> {
    return this.api.get<Category[]>('Categories');
  }

  getPaginated(page: number = 1, filter?: string): Observable<PaginationResponse<Category>> {
    return this.api.get<PaginationResponse<Category>>('Categories/paginated', { page, recordsPerPage: 10, filter });
  }

  create(category: CreateCategoryRequest): Observable<Category> {
    return this.api.post<Category>('Categories', category);
  }

  update(id: string, category: CreateCategoryRequest): Observable<Category> {
    return this.api.put<Category>(`Categories/${id}`, category);
  }
}