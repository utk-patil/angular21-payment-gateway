export interface PageResult<T> {
  pageNumber: number;
  pageSize: number;
  totalRecords: number;
  totalPages: number;
  data: T[];
}
