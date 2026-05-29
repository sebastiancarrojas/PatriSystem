export interface Product {
  id: string;
  productName: string;
  barcode: string | null;
  productDescription: string | null;
  unitPrice: number;
  unitOfMeasure: string | null;
  status: boolean;
  createdAt: string;
  updatedAt: string | null;
  categoryId: string;
  categoryName: string;
  brandId: string;
  brandName: string;
}

export interface CreateProductRequest {
  productName: string;
  barcode: string | null;
  productDescription: string | null;
  categoryId: string;
  brandId: string;
  unitPrice: number;
  unitOfMeasure: string | null;
}

export interface UpdateProductRequest {
  productName: string;
  barcode: string | null;
  productDescription: string | null;
  categoryId: string;
  brandId: string;
  unitPrice: number;
  unitOfMeasure: string | null;
}

export interface ProductSearch {
  id: string;
  productName: string;
  barcode: string | null;
  unitPrice: number;
  currentStock: number;
}