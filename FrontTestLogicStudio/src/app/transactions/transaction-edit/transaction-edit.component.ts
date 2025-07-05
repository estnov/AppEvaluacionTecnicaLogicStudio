import { Component, Inject, OnInit, inject } from '@angular/core';
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
import { NZ_MODAL_DATA, NzModalRef } from 'ng-zorro-antd/modal';

interface ProductRow extends ProductDto {
  checked: boolean;
  cantidad: number;
  detalle: string;
}

@Component({
  selector: 'app-transaction-edit',
  standalone: true,
  imports: [
    CommonModule, FormsModule,
    NzTableModule, NzCheckboxModule, NzInputNumberModule,
    NzInputModule, NzSelectModule, NzFormModule, NzButtonModule
  ],
  templateUrl: './transaction-edit.component.html',
  styleUrls: ['./transaction-edit.component.scss']
})
export class TransactionEditComponent implements OnInit {
  /* El modal nos pasa sólo el id */
  constructor(
    private modal: NzModalRef,
    @Inject(NZ_MODAL_DATA) public txId: number
  ) {}

  private prodSrv = inject(ProductsService);
  private txSrv   = inject(TransactionService);
  private msg     = inject(NzMessageService);

  txTypes: TransactionTypeDto[] = [];
  idTipoTransaccion: number | null = null;

  rows: ProductRow[] = [];

  ngOnInit(): void {
    forkJoin({
      products: this.prodSrv.getProducts$(),
      types   : this.txSrv.getTransactionTypes$(),
      tx      : this.txSrv.getTransaction$(this.txId)
    }).subscribe(({ products, types, tx }) =>  {
      this.txTypes          = types;
      this.idTipoTransaccion = tx.idTipoTransaccion;

      this.rows = products.map(p => {
        const det = tx.transaccionDetalles.find(d => d.idProducto === p.id);
        return {
          ...p,
          checked : !!det,
          cantidad: det?.cantidad ?? 1,
          detalle : det?.detalle  ?? ''
        };
      });
    });
  }

  /* helpers de cálculo idénticos al componente de creación */
  precioTotal = (r: ProductRow) => r.cantidad * r.precio;
  totalGeneral = () =>
    this.rows.filter(r => r.checked)
             .reduce((s,r)=> s+this.precioTotal(r),0);

  /* guardar (PUT) */
  save(): void {
    const detalles: DetalleTransaccionDto[] =
      this.rows.filter(r=>r.checked).map(r=>({
        id                     : 0,
        idTransaccionCabecera  : this.txId,
        idProducto             : r.id,
        cantidad               : r.cantidad,
        precioUnitario         : r.precio,
        precioTotal            : this.precioTotal(r),
        detalle                : r.detalle
      }));

    const payload: TransaccionDto = {
      id                 : this.txId,
      idTipoTransaccion  : this.idTipoTransaccion!,
      fecha              : new Date().toISOString(),
      transaccionDetalles: detalles
    };

    this.txSrv.updateTransaction$(this.txId, payload).subscribe({
      next: () => {
        this.msg.success('Transacción actualizada');
        this.modal.close(true);        // devolvemos bandera para refrescar lista
      },
      error: e => this.msg.error('Error al actualizar')
    });
  }
}
