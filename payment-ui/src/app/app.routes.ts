import { Routes } from '@angular/router';
import { PaymentComponent } from './features/payments/payment/payment';
import { TransactionsComponent } from './features/payments/transactions/transactions';
import { DemoPayComponent } from './features/payments/demo-pay/demo-pay';
import { PaymentSuccessComponent } from './features/payments/payment/success/success';
import { PaymentCancelComponent } from './features/payments/payment/cancel/cancel';

export const routes: Routes = [
  { path: '', redirectTo: 'payment', pathMatch: 'full' },
  { path: 'payment', component: PaymentComponent, runGuardsAndResolvers: 'always' },
  { path: 'transactions', component: TransactionsComponent, runGuardsAndResolvers: 'always' },
  { path: 'demo-pay/pay', component: DemoPayComponent, runGuardsAndResolvers: 'always' },
  { path: 'payment/success', component: PaymentSuccessComponent, runGuardsAndResolvers: 'always' },
  { path: 'payment/cancel', component: PaymentCancelComponent, runGuardsAndResolvers: 'always' },
];
