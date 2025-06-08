export interface PagedResult<T> {
    items: T[];
    totalCount: number;
    resultCount: number;
}