import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { PaymentService } from '../../../core/services/payment';

@Component({
  selector: 'app-demo-pay',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './demo-pay.html',
  styleUrl: './demo-pay.scss',
})
export class DemoPayComponent {
  orderId = signal('');
  amount = signal<number | null>(null);
  loading = signal(true);
  error = signal('');

  private token = '';

  constructor(route: ActivatedRoute, private paymentService: PaymentService) {
    const token = route.snapshot.queryParamMap.get('token');

    if (!token) {
      this.error.set('Invalid payment link');
      this.loading.set(false);
      return;
    }

    this.token = token;
    this.loadPaymentSession();
  }

  loadPaymentSession() {
    this.paymentService.getDemoPaySession(this.token).subscribe({
      next: (res) => {
        this.orderId.set(res.orderId);
        this.amount.set(res.amount);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Payment link expired or invalid');
        this.loading.set(false);
      },
    });
  }

  confirm(isSuccess: boolean) {
    if (!this.token) return;

    this.loading.set(true);

    this.paymentService
      .confirmPayment(this.token, isSuccess)
      .subscribe({
        next: (res) => {
          window.location.href = res.redirectUrl;
        },
        error: () => {
          this.loading.set(false);
          alert('Payment failed or session expired.');
        },
      });
  }

}
