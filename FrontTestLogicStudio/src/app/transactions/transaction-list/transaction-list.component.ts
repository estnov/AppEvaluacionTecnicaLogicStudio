import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  NzTableModule} from 'ng-zorro-antd/table';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzButtonModule } from 'ng-zorro-antd/button';
import {
  DetalleTransaccionDto,
  TransaccionDto,
  TransactionRow,
  TransactionTypeDto
} from '../../../interfaces/Interfaces';
import { TransactionService } from '../../services/transaction.service';
import { forkJoin } from 'rxjs';
import { NzSelectModule } from 'ng-zorro-antd/select'; 
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-transaction-list',
  imports: [
    FormsModule,
    NzTableModule,
    NzInputModule,
    NzIconModule,
    NzDropDownModule,
    NzButtonModule,
    NzSelectModule,
    CommonModule 
   ],
  templateUrl: './transaction-list.component.html',
  styleUrl: './transaction-list.component.scss'
})
export class TransactionListComponent {
  listOfData: TransactionRow[] = [];
  listOfDisplayData: TransactionRow[] = [];

  searchValue = '';
  selectedTypeId: number | null = null;
  visible = false;
  listOfTypes: TransactionTypeDto[] = [];  
  typeMap: Record<number, string> = {};

  constructor(private txService: TransactionService) {}


  ngOnInit(): void {
    forkJoin({
      types: this.txService.getTransactionTypes$(),
      txs: this.txService.getTransactionList$()
    }).subscribe(({ types, txs }) => {
      this.listOfTypes = types;
      this.typeMap = Object.fromEntries(
        types.map(t => [t.id, t.nombre])
      );

      this.listOfData = this.listOfDisplayData = txs;
    });
  }

  reset(): void {
    this.selectedTypeId = null;
    this.search();
  }

  search(): void {
    this.visible = false;
    this.listOfDisplayData =
      this.selectedTypeId == null
        ? [...this.listOfData]
        : this.listOfData.filter(
            tx => tx.idTipoTransaccion === this.selectedTypeId
          );
  }
}
