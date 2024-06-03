using MediatR;
using Shared;

namespace Application.Features.Companies.Queries.CompanyPaginatedAllList
{
    /// <summary>
    /// Represents the query for getting a paginated list of GetCompanyAllListDto.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response, which is PaginatedResult<GetCompanyAllListDto>.</typeparam>
    public class GetCompanyPaginatedAllListQuery : IRequest<PaginatedResult<GetCompanyPaginatedAllListDto>>
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

        public GetCompanyPaginatedAllListQuery() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCompanyPaginatedAllListQuery"/> class.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        public GetCompanyPaginatedAllListQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
