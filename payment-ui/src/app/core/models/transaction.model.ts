export interface Transaction {
  id: number;
  orderId: string;
  providerReference?: string;
  amount: number;
  status: string;
  updatedOn?: string;
}