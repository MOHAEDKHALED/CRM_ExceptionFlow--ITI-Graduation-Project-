namespace CRM_ExceptionFlow.DTOs.Common
{
    public class PagedResult<T>
    {
        public IReadOnlyCollection<T> Items { get; init; } = Array.Empty<T>();
        public int TotalItems { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }

        public static PagedResult<T> From(IReadOnlyCollection<T> items, int totalItems, int pageNumber, int pageSize)
        {
            return new PagedResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}

