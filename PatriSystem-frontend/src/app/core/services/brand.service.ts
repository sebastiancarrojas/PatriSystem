import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Brand, CreateBrandRequest } from '../models/brand.model';
import { PaginationResponse } from '../models/pagination.model';

@Injectable({ providedIn: 'root' })
export class BrandService {
  private api = inject(ApiService);

  getAll(): Observable<Brand[]> {
    return this.api.get<Brand[]>('Brands');
  }

  getPaginated(page: number = 1, filter?: string): Observable<PaginationResponse<Brand>> {
    return this.api.get<PaginationResponse<Brand>>('Brands/paginated', { page, recordsPerPage: 10, filter });
  }

  create(brand: CreateBrandRequest): Observable<Brand> {
    return this.api.post<Brand>('Brands', brand);
  }

  update(id: string, brandName: string, brandDescription?: string): Observable<Brand> {
    return this.api.put<Brand>(`Brands/${id}`, { brandName, brandDescription });
  }
}