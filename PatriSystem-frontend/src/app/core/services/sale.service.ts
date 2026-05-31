import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { ApiResponse } from '../models/api-response.model';
import { Sale, CreateSaleRequest} from '../models/sale.model';
import { PaginationResponse } from '../models/pagination.model';
import { SalePaginationRequest } from '../models/pagination.model';


@Injectable({ providedIn: 'root' })
export class SaleService {
  private api = inject(ApiService);

  getAll(): Observable<Sale[]> {
    return this.api.get<Sale[]>('Sales');
  }

  getPaginated(request: SalePaginationRequest): Observable<PaginationResponse<Sale>> {
  return this.api.get<PaginationResponse<Sale>>('Sales/paginated', request as Record<string, any>);
  }

  getById(id: string): Observable<Sale> {
    return this.api.get<Sale>(`Sales/${id}`);
  }

  create(sale: CreateSaleRequest): Observable<ApiResponse<string>> {
    return this.api.post<ApiResponse<string>>('Sales', sale);
  }
}