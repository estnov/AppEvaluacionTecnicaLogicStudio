import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { forkJoin } from 'rxjs';

import { CommonModule }          from '@angular/common';
import { NzTableModule }         from 'ng-zorro-antd/table';
import { NzCheckboxModule }      from 'ng-zorro-antd/checkbox';
import { NzInputNumberModule }   from 'ng-zorro-antd/input-number';
import { NzInputModule }         from 'ng-zorro-antd/input';
import { NzSelectModule }        from 'ng-zorro-antd/select';
import { NzButtonModule }        from 'ng-zorro-antd/button';
import { NzMessageService }      from 'ng-zorro-antd/message';

import {
  ProductDto
} from '../../../interfaces/Interfaces';
import {
  TransaccionDto,
  DetalleTransaccionDto,
  TransactionTypeDto
} from '../../../interfaces/Interfaces';
import { ProductsService } from '../../../app/services/products.service';
import { TransactionService } from '../../../app/services/transaction.service';
import { NzFormModule } from 'ng-zorro-antd/form';

interface ProductRow extends ProductDto {
  checked: boolean;
  cantidad: number;
  detalle: string;
}

@Component({
  selector: 'app-transaction',
  imports: [
    CommonModule,
    FormsModule,
    NzTableModule,
    NzCheckboxModule,
    NzInputNumberModule,
    NzInputModule,
    NzSelectModule,
    NzFormModule,
    NzButtonModule
  ],
  templateUrl: './transaction.component.html',
  styleUrl: './transaction.component.scss'
})
export class TransactionComponent implements OnInit{
private prodSrv = inject(ProductsService);
  private txSrv   = inject(TransactionService);
  private msg     = inject(NzMessageService);

  txTypes: TransactionTypeDto[] = [];
  idTipoTransaccion: number | null = null;

  rows: ProductRow[] = [];

  ngOnInit(): void {
    forkJoin({
      products: this.prodSrv.getProducts$(),
      types:    this.txSrv.getTransactionTypes$()
    }).subscribe(({ products, types }) => {
      this.rows = products.map(p => ({
        ...p,
        checked: false,
        cantidad: 1,
        detalle: ''
      }));
      this.txTypes = types;
    });
  }

  precioTotal(row: ProductRow): number {
    return row.cantidad * row.precio;
  }

  totalGeneral(): number {
    return this.rows
      .filter(r => r.checked)
      .reduce((s, r) => s + this.precioTotal(r), 0);
  }

  save(): void {
    if (this.idTipoTransaccion === null) {
      this.msg.error('Seleccione el tipo de transacción');
      return;
    }
    const seleccionados = this.rows.filter(r => r.checked);
    if (seleccionados.length === 0) {
      this.msg.error('Seleccione al menos un producto');
      return;
    }

    const detalles: DetalleTransaccionDto[] = seleccionados.map(r => ({
      id: 0,
      idTransaccionCabecera: 0,
      idProducto: r.id,
      cantidad: r.cantidad,
      precioUnitario: r.precio,
      precioTotal: this.precioTotal(r),
      detalle: r.detalle || ''
    }));

    const payload: TransaccionDto = {
      id: 0,
      idTipoTransaccion: this.idTipoTransaccion,
      fecha: new Date().toISOString(),
      transaccionDetalles: detalles
    };

    this.txSrv.generateTransaction$(payload).subscribe({
      next: () => {
        this.msg.success('Transacción registrada');
        this.idTipoTransaccion = null;
        this.rows.forEach(r => (r.checked = false));
      },
      error: e => {
        console.error(e);
        this.msg.error('Error al registrar');
      }
    });
  }
}
