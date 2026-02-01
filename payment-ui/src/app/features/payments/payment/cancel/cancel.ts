import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-payment-cancel',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './cancel.html',
  styleUrl: './cancel.scss',
})
export class PaymentCancelComponent {
  message = signal('Payment Cancelled ❌');
}
