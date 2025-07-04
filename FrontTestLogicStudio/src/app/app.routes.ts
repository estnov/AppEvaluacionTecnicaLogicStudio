import { Routes } from '@angular/router';
import { TransactionListComponent } from './transactions/transaction-list/transaction-list.component';
import { TransactionComponent } from './transactions/transaction/transaction.component';
import { ProductsAdministrationComponent } from './products/products-administration/products-administration.component';
import { ProductComponent } from './products/product/product.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/welcome' },
  { path: 'welcome', loadChildren: () => import('./pages/welcome/welcome.routes').then(m => m.WELCOME_ROUTES) },
  { path: 'transaction-list', component: TransactionListComponent},
  { path: 'transaction', component: TransactionComponent },
  { path: 'product-administration', component: ProductsAdministrationComponent},
  { path: 'product', component: ProductComponent}
];
