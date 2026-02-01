import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaymentService } from '../../../core/services/payment';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './payment.html',
  styleUrl: './payment.scss',
})
export class PaymentComponent {
  orderId = signal('');
  amount = signal<number | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);

  constructor(private paymentService: PaymentService) {}

  pay(form: any) {
    if (form.invalid) return;

    this.loading.set(true);
    this.error.set(null);

    this.paymentService
      .createPayment(this.orderId(), this.amount()!)
      .subscribe({
        next: (res) => {
          window.location.href = res.paymentUrl;
        },
        error: () => {
          this.error.set('Failed to create payment');
          this.loading.set(false);
        },
      });
  }
}
