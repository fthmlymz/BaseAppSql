using Application.Features.KKCs.Commands.CreateKKC;
using Application.Features.KKCs.Commands.DeleteKKC;
using Application.Features.KKCs.Commands.UpdateKKC;
using Application.Features.KKCs.Queries.KKCSearchWithPagination;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KKCController : ControllerBase
    {
        private readonly IMediator _mediator;

        public KKCController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize("KKCCreateRole")]
        [HttpPost]
        public async Task<IActionResult> CreateKKCCommand(CreateKKCCommand command, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(command, cancellationToken));
        }


        //[Authorize("KKCReadRole")]
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<PaginatedResult<GetKKCSearchWithPaginationDto>>> GetCompanySearchQuery([FromQuery] GetKKCSearchWithPaginationQuery dto, CancellationToken cancellationToken)
        {
            return await _mediator.Send(dto);
        }

       // [Authorize("KKCUpdateRole")]
        [HttpPut]
        public async Task<ActionResult<Result<KKC>>> UpdateKKCCommand([FromBody] UpdateKKCCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        //[Authorize("KKCDeleteRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<NoContent>>> DeleteKCCCommand(int id)
        {
            await _mediator.Send(new DeleteKCCCommand(id));
            return NoContent();
        }
    }
}
