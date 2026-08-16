import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { UnitOfMeasure, CreateUnitOfMeasureRequest } from '../models/unit-of-measure.model';

@Injectable({ providedIn: 'root' })
export class UnitOfMeasureService {
  private api = inject(ApiService);

  getAll(): Observable<UnitOfMeasure[]> {
    return this.api.get<UnitOfMeasure[]>('UnitsOfMeasure');
  }

  create(unitOfMeasure: CreateUnitOfMeasureRequest): Observable<UnitOfMeasure> {
    return this.api.post<UnitOfMeasure>('UnitsOfMeasure', unitOfMeasure);
  }
}