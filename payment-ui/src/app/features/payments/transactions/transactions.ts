import { Component, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaymentService } from '../../../core/services/payment';
import { Transaction } from '../../../core/models/transaction.model';
import { LocalDateTimePipe } from '../../../core/pipes/local-datetime.pipe';

let debounceTimer: any;

@Component({
  selector: 'app-transactions',
  standalone: true,
  imports: [CommonModule, LocalDateTimePipe],
  templateUrl: './transactions.html',
  styleUrl: './transactions.scss',
})
export class TransactionsComponent {
  transactions = signal<Transaction[]>([]);
  loading = signal(false);

  pageNumber = signal(1);
  pageSize = 10;
  totalPages = signal(0);
  totalRecords = signal(0);

  search = signal('');
  status = signal('');

  constructor(private paymentService: PaymentService) {

    effect(() => {
      this.load(this.pageNumber());
    });

    effect(() => {
      const s = this.search();
      const st = this.status();

      clearTimeout(debounceTimer);
      debounceTimer = setTimeout(() => {
        this.pageNumber.set(1);
        this.load(1);
      }, 400);
    });
  }

  load(page: number) {
    this.loading.set(true);

    this.paymentService
      .getTransactions(page, this.pageSize, this.search(), this.status()
      )
      .subscribe({
        next: (res) => {
          this.transactions.set(res.data);
          this.totalPages.set(res.totalPages);
          this.totalRecords.set(res.totalRecords);
          this.loading.set(false);
        },
        error: () => {
          this.loading.set(false);
        },
      });
  }

  clearFilter() {
    this.search.set('');
    this.status.set('');
    this.pageNumber.set(1);
    this.load(1);
  }

  next() {
    if (this.pageNumber() < this.totalPages()) {
      this.pageNumber.update(p => p + 1);
    }
  }

  prev() {
    if (this.pageNumber() > 1) {
      this.pageNumber.update(p => p - 1);
    }
  }

  repay(t: Transaction) {
    this.paymentService
      .repay(t.orderId)
      .subscribe({
        next: (res) => {
          window.location.href = res.paymentUrl;
        },
        error: () => {
          alert('Unable to retry payment');
        },
      });
  }

}
