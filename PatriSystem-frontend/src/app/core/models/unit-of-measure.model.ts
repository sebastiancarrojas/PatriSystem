export interface UnitOfMeasure {
  id: string;
  name: string;
  createdAt: string;
  updatedAt: string | null;
}

export interface CreateUnitOfMeasureRequest {
  name: string;
}