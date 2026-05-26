import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { ApiResponse } from '../models/api-response.model';
import { Brand, CreateBrandRequest } from '../models/brand.model';

@Injectable({ providedIn: 'root' })
export class BrandService {
  private api = inject(ApiService);

  getAll(): Observable<Brand[]> {
    return this.api.get<Brand[]>('Brands');
  }

  create(brand: CreateBrandRequest): Observable<ApiResponse<object>> {
    return this.api.post<ApiResponse<object>>('Brands', brand);
  }
}