export interface SaleDetail {
  id: string;
  productId: string;
  productName: string;
  quantity: number;
  unitPrice: number;
  subTotal: number;
}

export interface Sale {
  id: string;
  saleDate: string;
  totalAmount: number;
  saleDetails: SaleDetail[];
}

export interface CreateSaleDetailRequest {
  productId?: string;
  quantity: number;
  unitPrice: number;
  isTemporary: boolean;
  productName?: string;
}

export interface CreateSaleRequest {
  details: CreateSaleDetailRequest[];
}

