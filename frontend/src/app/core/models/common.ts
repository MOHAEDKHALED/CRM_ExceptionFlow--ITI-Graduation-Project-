export interface PagedResult<T> {
  items: T[];
  totalItems: number;
  pageNumber: number;
  pageSize: number;
}

