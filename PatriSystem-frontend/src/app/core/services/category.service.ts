import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { ApiResponse } from '../models/api-response.model';
import { Category, CreateCategoryRequest } from '../models/category.model';

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private api = inject(ApiService);

  getAll(): Observable<Category[]> {
    return this.api.get<Category[]>('Categories');
  }

  create(category: CreateCategoryRequest): Observable<ApiResponse<object>> {
    return this.api.post<ApiResponse<object>>('Categories', category);
  }
}