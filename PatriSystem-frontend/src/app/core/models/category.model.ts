export interface Category {
  id: string;
  categoryName: string;
  createdAt: string;
  updatedAt: string | null;
}

export interface CreateCategoryRequest {
  categoryName: string;
}