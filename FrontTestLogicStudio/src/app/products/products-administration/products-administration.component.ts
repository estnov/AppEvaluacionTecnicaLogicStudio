import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { forkJoin } from 'rxjs';

import { CommonModule }      from '@angular/common';
import { NzTableModule }     from 'ng-zorro-antd/table';
import { NzInputModule }     from 'ng-zorro-antd/input';
import { NzIconModule }      from 'ng-zorro-antd/icon';
import { NzDropDownModule }  from 'ng-zorro-antd/dropdown';
import { NzButtonModule }    from 'ng-zorro-antd/button';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';

import {
  ProductDto,
  CategoryDto,
  ProductUpsertDto
} from '../../../interfaces/Interfaces';
import { ProductsService } from '../../../app/services/products.service';
import { NzMessageService } from 'ng-zorro-antd/message';

export type ProductRow = ProductDto & { categoria: string };

@Component({
  selector: 'app-products-administration',
  imports: [
    CommonModule,
    FormsModule,
    NzTableModule,
    NzInputModule,
    NzIconModule,
    NzDropDownModule,
    NzButtonModule,
    NzPopconfirmModule],
  templateUrl: './products-administration.component.html',
  styleUrl: './products-administration.component.scss'
})
export class ProductsAdministrationComponent {
private prodSrv = inject(ProductsService);
  private msg     = inject(NzMessageService);

  listOfData: ProductRow[] = [];
  listOfDisplayData: ProductRow[] = [];

  searchValue = '';
  visible = false;

  editingKey: string | null = null;
  editValue = '';

  catMap: Record<number, string> = {};

  ngOnInit(): void {
    forkJoin({
      cats: this.prodSrv.getCategories$(),
      prods: this.prodSrv.getProducts$()
    }).subscribe(({ cats, prods }) => {
      this.catMap = Object.fromEntries(cats.map(c => [c.id, c.descripcion]));
      this.listOfData = this.listOfDisplayData = prods.map(p => ({
        ...p,
        categoria: this.catMap[p.idCategoria] ?? '—'
      }));
    });
  }
  reset(): void { this.searchValue = ''; this.search(); }

  search(): void {
    this.visible = false;
    const val = this.searchValue.trim().toLowerCase();
    this.listOfDisplayData =
      val.length === 0
        ? [...this.listOfData]
        : this.listOfData.filter(p =>
            p.nombre.toLowerCase().includes(val)
          );
  }

  keyOf(row: ProductRow, field: keyof ProductRow) {
    return `${row.id}-${field}`;
  }

  startEdit(row: ProductRow, field: keyof ProductRow): void {
    this.editingKey = this.keyOf(row, field);
    this.editValue  = String(row[field] ?? '');
  }

  saveEdit(row: ProductRow, field: keyof ProductRow): void {
    const key = this.keyOf(row, field);
    if (this.editingKey !== key) 
    { 
      return; 
    }

    if (String(row[field] ?? '') === this.editValue) {
      this.editingKey = null;
      return;
    }

    (row as any)[field] = field === 'precio' || field === 'stock'
      ? +this.editValue
      : this.editValue;

    const { categoria, ...dto }: ProductRow & { categoria?: string } = row;
    this.prodSrv.updateProduct$(row.id, dto as ProductUpsertDto).subscribe({
      next: () => this.msg.success('Actualizado'),
      error: e => {
        this.msg.error('Error al actualizar');
        console.error(e);
      }
    });

    this.editingKey = null;
  }
  delete(row: ProductRow): void {
    this.prodSrv.deleteProduct$(row.id).subscribe({
      next: () => {
        this.listOfData = this.listOfData.filter(p => p.id !== row.id);
        this.search();
        this.msg.success('Eliminado');
      },
      error: e => {
        this.msg.error('No se pudo eliminar');
        console.error(e);
      }
    });
  }
}
