using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using MP.BLL.Helpers;
using MP.Domain.Model;
using MP.Domain.Object;

namespace MP.BLL.UseCases.AddTicket;

public static partial class AddTicketUseCase
{
    public class Handler : IRequestHandler<Command, Result<QueryResult>>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IManager _ticketManager;

        public Handler(IManager ticketManager, ILogger<Handler> logger)
        {
            _ticketManager = ticketManager;
            _logger = logger;
        }

        public Task<Result<QueryResult>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var visitDateUtc = DateTimeHelper.ConvertToUtc(request.Model.VisitDate, request.TimeZoneId);
                
                var ticket = new Ticket
                {
                    Title = request.Model.Title,
                    Description = request.Model.Description,
                    VisitDate = visitDateUtc,
                    VisitorsNumber = request.Model.VisitorsNumber
                };
            
                _ticketManager.AddTicket(ticket);

                return Task.FromResult(Result.Ok(new QueryResult(ticket.ID ?? -1)));
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occured in SaveTicket. reason={reason}", e.Message);
                
                return Task.FromResult<Result<QueryResult>>(Result.Fail("Не удалось добавить заявку!"));
            }
        }
    }
}