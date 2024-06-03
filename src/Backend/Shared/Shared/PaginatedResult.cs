namespace Shared
{
    /// <summary>
    /// Represents a paginated result of type T.
    /// </summary>
    /// <typeparam name="T">The type of the data in the result.</typeparam>
    public class PaginatedResult<T> : Result<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedResult{T}"/> class.
        /// </summary>
        public PaginatedResult()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedResult{T}"/> class.
        /// </summary>
        /// <param name="data">The data of type T.</param>
        public PaginatedResult(IReadOnlyList<T> data)
        {
            Data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedResult{T}"/> class.
        /// </summary>
        /// <param name="succeeded">A value indicating whether the operation succeeded.</param>
        /// <param name="data">The data of type T. Default value is null.</param>
        /// <param name="messages">The list of messages. Default value is null.</param>
        /// <param name="count">The total count of items.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The size of each page.</param>
        public PaginatedResult(bool succeeded, IReadOnlyList<T> data = default, List<string> messages = null, int count = 0, int pageNumber = 1, int pageSize = 10)
        {
            Data = data;
            CurrentPage = pageNumber;
            Succeeded = succeeded;
            Messages = messages;
            PageSize = pageSize;
            TotalPages = (count + pageSize - 1) / pageSize;
            TotalCount = count;
            HasPreviousPage = CurrentPage > 1;
            HasNextPage = CurrentPage < TotalPages;
        }

        /// <summary>
        /// Gets or sets the data of type T.
        /// </summary>
        public IReadOnlyList<T> Data { get; set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the total count of items.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the size of each page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a previous page.
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a next page.
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="PaginatedResult{T}"/> class with the specified data, count, page number, and page size.
        /// </summary>
        /// <param name="data">The data of type T.</param>
        /// <param name="count">The total count of items.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The size of each page.</param>
        /// <returns>A new instance of the <see cref="PaginatedResult{T}"/> class.</returns>
        public static PaginatedResult<T> Create(IReadOnlyList<T> data, int count, int pageNumber, int pageSize)
        {
            return new PaginatedResult<T>(true, data, null, count, pageNumber, pageSize);
        }
    }
}
