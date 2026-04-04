export interface Product {
  id: number;
  name: string;
  description: string;
  createdAt: string;
  updatedAt: string | null;
  suppliers: ProductSupplier[];
}

export interface ProductSupplier {
  id: number;
  supplierId: number;
  supplierName: string;
  price: number;
  stock: number;
  batchNumber: string;
}

export interface CreateProduct {
  name: string;
  description: string;
  suppliers: CreateProductSupplier[];
}

export interface CreateProductSupplier {
  supplierId: number;
  price: number;
  stock: number;
  batchNumber: string;
}