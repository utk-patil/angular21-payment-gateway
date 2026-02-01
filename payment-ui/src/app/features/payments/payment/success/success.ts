import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-payment-success',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './success.html',
  styleUrl: './success.scss',
})
export class PaymentSuccessComponent {
  message = signal('Payment Successful 🎉');
}
