export interface Brand {
  id: string;
  brandName: string;
  brandDescription: string | null;
  createdAt: string;
  updatedAt: string | null;
}

export interface CreateBrandRequest {
  brandName: string;
  brandDescription: string | null;
}