import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  CategoryDto,
  ProductDto,
  ProductUpsertDto
} from '../../interfaces/Interfaces';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {

  private readonly baseUrl = 'http://localhost:8080/api/Products';

  constructor(private http: HttpClient) {}

  getCategories$(): Observable<CategoryDto[]> {
    return this.http.get<CategoryDto[]>(`${this.baseUrl}/GetCategoriesList`);
  }

  getProducts$(): Observable<ProductDto[]> {
    return this.http.get<ProductDto[]>(`${this.baseUrl}/GetProductsList`);
  }

  getProduct$(id: number): Observable<ProductDto> {
    return this.http.get<ProductDto>(`${this.baseUrl}/GetProduct/${id}`);
  }

  createProduct$(payload: ProductUpsertDto): Observable<number> {
    return this.http.put<number>(
      `${this.baseUrl}/CreateProduct`,
      payload
    );
  }

  updateProduct$(id: number, payload: ProductUpsertDto): Observable<void> {
    return this.http.put<void>(
      `${this.baseUrl}/UpdateProduct/${id}`,
      payload
    );
  }

  deleteProduct$(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/DeleteProduct/${id}`);
  }
}
