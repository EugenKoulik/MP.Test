using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MP.API.Extensions;
using MP.BLL.UseCases.AddTicket;
using MP.BLL.UseCases.GetTickets;

namespace MP.API.Controllers;

[ApiController]
[Route("ticket")]
[Produces(MediaTypeNames.Application.Json)]
public class TicketController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public TicketController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetTickets(DateTime from, DateTime to, string timeZoneId, CancellationToken cancellationToken)
    {
        var command = new GetTicketUseCase.Command(from, to, timeZoneId);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return result.ToActionResult();
    }
    
    [HttpPost("save")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> SaveTicket([FromBody] AddTicketUseCase.Command request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToActionResult();
    }
}