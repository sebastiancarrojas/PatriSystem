export interface PaginationResponse<T> {
  currentPage: number;
  totalPages: number;
  recordsPerPage: number;
  totalCount: number;
  hasPrevious: boolean;
  hasNext: boolean;
  filter: string | null;
  items: T[];
}

export interface ProductPaginationRequest {
  page?: number;
  recordsPerPage?: number;
  filter?: string;
  categoryId?: string;
  brandId?: string;
  status?: boolean;
}