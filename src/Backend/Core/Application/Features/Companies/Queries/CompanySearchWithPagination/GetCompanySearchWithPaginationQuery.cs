using MediatR;
using Shared;

namespace Application.Features.Companies.Queries.CompanySearchWithPagination
{
    /// <summary>
    /// Represents the query for getting a paginated list of CompanySearchWithPaginationDto.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response, which is PaginatedResult<GetCompanySearchWithPaginationDto>.</typeparam>
    public class GetCompanySearchWithPaginationQuery : IRequest<PaginatedResult<GetCompanySearchWithPaginationDto>>
    {
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <value>The page number.</value>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        /// <value>The page size.</value>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string? Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCompanySearchWithPaginationQuery"/> class.
        /// </summary>
        public GetCompanySearchWithPaginationQuery() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCompanySearchWithPaginationQuery"/> class.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="name">The name.</param>
        public GetCompanySearchWithPaginationQuery(int pageNumber, int pageSize, string? name)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Name = name;
        }
    }
}
