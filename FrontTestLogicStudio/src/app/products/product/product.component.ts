import { Component, NgZone, OnDestroy, OnInit, inject  } from '@angular/core';
import {
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';

import { CommonModule }   from '@angular/common';
import { NzFormModule }   from 'ng-zorro-antd/form';
import { NzInputModule }  from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzUploadFile, NzUploadModule } from 'ng-zorro-antd/upload';
import { NzMessageService } from 'ng-zorro-antd/message';

import { CategoryDto, ProductUpsertDto } from '../../../interfaces/Interfaces';
import { ProductsService } from '../../services/products.service';
import { NzIconModule } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-product',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzSelectModule,
    NzButtonModule,
    NzUploadModule,
    NzIconModule          
  ],
  templateUrl: './product.component.html',
  styleUrl: './product.component.scss'
})
export class ProductComponent {

  private fb = inject(NonNullableFormBuilder);
  private destroy$ = new Subject<void>();
  private msg = inject(NzMessageService);
  private productsSrv = inject(ProductsService);
  private zone = inject(NgZone);
  previewImg: string | null = null;  

  categories: CategoryDto[] = [];

  form = this.fb.group({
    nombre:       ['', [Validators.required]],
    descripcion:  ['', [Validators.required]],
    precio:       [0,  [Validators.required, Validators.min(0.01)]],
    stock:        [0,  [Validators.required, Validators.min(0)]],
    idCategoria:  [null as number | null, [Validators.required]],
    imagenBase64: ['0x'] 
  });

  ngOnInit(): void {
    this.productsSrv.getCategories$()
      .pipe(takeUntil(this.destroy$))
      .subscribe(cat => (this.categories = cat));
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  handleFile = (file: NzUploadFile | File): boolean => {
    const rawFile: File = (file as NzUploadFile).originFileObj ?? (file as File);

    if (!rawFile) { return false; }

    const reader = new FileReader();
    reader.onload = () => this.zone.run(() => {
      this.previewImg = reader.result as string;
      this.form.patchValue({ imagenBase64: this.previewImg });
    });
    reader.readAsDataURL(rawFile);

    return false;       
  };

  
  submit(): void {
    if (this.form.invalid) {
      Object.values(this.form.controls).forEach(c => {
        c.markAsDirty();
        c.updateValueAndValidity();
      });
      return;
    }

    const raw = this.form.getRawValue();

    if (raw.idCategoria === null) {
      this.msg.error('Seleccione una categoría');
      return;
    }

    const { imagenBase64, ...rest } = raw;
    const payload: ProductUpsertDto = {
      ...rest,
      imagen: imagenBase64 ?? ''
    } as ProductUpsertDto;  

    console.log(payload)
    
    this.productsSrv.createProduct$(payload).subscribe({
      next: () => {
        this.msg.success('Producto creado');
        this.form.reset();
      },
      error: err => this.msg.error('Error al crear: ' + err.message)
    });
  }
}
