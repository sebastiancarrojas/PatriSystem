export interface Product {
  id: string;
  productName: string;
  barcode: string | null;
  productDescription: string | null;
  unitPrice: number;
  status: boolean;
  createdAt: string;
  updatedAt: string | null;
  categoryId: string;
  categoryName: string;
  brandId: string;
  brandName: string;
  unitOfMeasureId: string | null;
  unitOfMeasureName: string | null;
}

export interface CreateProductRequest {
  productName: string;
  barcode: string | null;
  productDescription: string | null;
  categoryId: string;
  brandId: string;
  unitPrice: number;
  unitOfMeasureId: string | null;
}

export interface UpdateProductRequest {
  productName: string;
  barcode: string | null;
  productDescription: string | null;
  categoryId: string;
  brandId: string;
  unitPrice: number;
  unitOfMeasureId: string | null;
}

export interface ProductSearch {
  id: string;
  productName: string;
  barcode: string | null;
  unitPrice: number;
  currentStock: number;
}