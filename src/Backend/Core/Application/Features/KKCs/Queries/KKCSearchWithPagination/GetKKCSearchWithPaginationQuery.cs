using MediatR;
using Shared;

namespace Application.Features.KKCs.Queries.KKCSearchWithPagination
{
    public class GetKKCSearchWithPaginationQuery : IRequest<PaginatedResult<GetKKCSearchWithPaginationDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        #region Filter Parameters
        public string? Name { get; set; }
        public int? DeviceId { get; set; }
        public string? DeviceIp { get; set; }
        #endregion

        public GetKKCSearchWithPaginationQuery() { }

        public GetKKCSearchWithPaginationQuery(int pageNumber, int pageSize, string? name, int? deviceId, string? deviceIp)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Name = name;
            DeviceId = deviceId;
            DeviceIp = deviceIp;
        }
    }
}
