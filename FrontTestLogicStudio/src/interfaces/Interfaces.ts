export interface DetalleTransaccionDto {
  id: number;
  idTransaccionCabecera: number;
  idProducto: number;
  cantidad: number;
  precioUnitario: number;
  precioTotal: number;
  detalle?: string;
}

export interface TransaccionDto {
  id: number;
  idTipoTransaccion: number;
  fecha: string;
  transaccionDetalles: DetalleTransaccionDto[];
}

export interface TransactionTypeDto {
    id: number;
    nombre: string;
}

export type TransactionRow = TransaccionDto & { total: number };

export interface CategoryDto {
  id: number;
  descripcion: string;
}

export interface ProductDto {
  id: number;
  nombre: string;
  descripcion: string;
  precio: number;
  imagen: string;
  stock: number;
  idCategoria: number;
}

export type ProductUpsertDto = Omit<ProductDto, 'id'> | ProductDto;