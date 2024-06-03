using Application.Features.Companies.Commands.CreateCompany;
using Application.Features.Companies.Commands.DeleteCompany;
using Application.Features.Companies.Commands.UpdateCompany;
using Application.Features.Companies.Queries.CompanyAllList;
using Application.Features.Companies.Queries.CompanyPaginatedAllList;
using Application.Features.Companies.Queries.CompanySearchWithPagination;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared;
using Web.API.middleware;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase //ApiControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new company.
        /// </summary>
        /// <param name="command">The command to create a new company. Type: CreateCompanyCommand</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains a Result of CreatedCompanyDto.</returns>
        [HttpPost]
        public async Task<ActionResult<Result<CreatedCompanyDto>>> CreateCompanyCommand(CreateCompanyCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Updates a company command.
        /// </summary>
        /// <param name="command">The update company command.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result of the update operation.</returns>
        //[Authorize("CompanyUpdateRole")]
        [HttpPut]
        public async Task<ActionResult<Result<NoContent>>> UpdateCompanyCommand([FromBody] UpdateCompanyCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Deletes a company by its id.
        /// </summary>
        /// <param name="id">The id of the company to delete. Type: int</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains a Result of NoContent.</returns>
        //[Authorize("CompanyDeleteRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<NoContent>>> DeleteCompanyCommand(int id)
        {
            await _mediator.Send(new DeleteCompanyCommand(id));
            return NoContent();
        }

        /// <summary>
        /// Retrieves a paginated list of companies based on the provided search query.
        /// </summary>
        /// <param name="dto">The search query parameters.</param>
        /// <returns>A paginated result of companies.</returns>
        //[Authorize("CompanyReadRole")]
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<PaginatedResult<GetCompanySearchWithPaginationDto>>> GetCompanySearchQuery([FromQuery] GetCompanySearchWithPaginationQuery dto)
        {
            return await _mediator.Send(dto);
        }

        /// <summary>
        /// Retrieves a paginated list of all companies.
        /// </summary>
        /// <param name="dto">The query parameters. Type: GetCompanyAllListQuery</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains a PaginatedResult of GetCompanyAllListDto.</returns>
        //[Authorize("CompanyReadRole")]
        [HttpGet]
        [Route("companyAllListPagination")]
        [RateLimit(MaxRequests = 3, TimeWindowInSeconds = 6)]
        public async Task<ActionResult<PaginatedResult<GetCompanyPaginatedAllListDto>>> GetCompanyAllList([FromQuery] GetCompanyPaginatedAllListQuery dto)
        {
            return await _mediator.Send(dto);
        }


        /// <summary>
        /// Retrieves a list of all companies.
        /// </summary>
        /// <returns>A Task that represents the asynchronous operation. The task result contains a Result of List of GetCompanyAllListDto.</returns>
        //[Authorize("CompanyReadRole")]
        [HttpGet]
        [Route("companyAllList")]
        public async Task<ActionResult<Result<List<GetCompanyAllListDto>>>> CompanyAllList()
        { 
            return await _mediator.Send(new GetCompanyAllListQuery());
        }

        ////Frontend - product/product-list for view
        //[Authorize("ProductReadRole")]
        //[HttpGet]
        //[Route("companylist")]
        //public async Task<ActionResult<Result<List<GetCompanyAllListDto>>>> GetCompanyList()
        //{
        //    return await _mediator.Send(new GetCompanyAllListQuery());
        //}
    }
}
