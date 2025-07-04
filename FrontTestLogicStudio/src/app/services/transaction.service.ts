import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import {
  TransaccionDto,
  TransactionRow,
  TransactionTypeDto
} from '../../interfaces/Interfaces';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  private readonly baseUrl = 'http://localhost:8081/api/Transactions';

  constructor(private http: HttpClient) {}

  getTransactionList$(): Observable<TransactionRow[]> {
    return this.http
      .get<TransaccionDto[]>(`${this.baseUrl}/GetTransactionList`)
      .pipe(
        map(list =>
          list.map(tx => ({
            ...tx,
            total: tx.transaccionDetalles
              .reduce((sum, d) => sum + d.precioTotal, 0)
          }))
        )
      );
  }

  getTransactionTypes$(): Observable<TransactionTypeDto[]> {
    return this.http.get<TransactionTypeDto[]>(
      `${this.baseUrl}/GetTransactionTypes`
    );
  }

  generateTransaction$(payload: TransaccionDto): Observable<void> {
    return this.http.post<void>(
      `${this.baseUrl}/GenerateTransaction`,
      payload
    );
  }
}
