using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using MP.BLL.Helpers;
using MP.Domain.Object;
using MP.Model.WS;
using NodaTime;

namespace MP.BLL.UseCases.GetTickets;

public static partial class GetTicketUseCase
{
    public class Handler : IRequestHandler<Command, Result<QueryResult>>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IManager _ticketManager;

        public Handler(ILogger<Handler> logger, IManager ticketManager)
        {
            _logger = logger;
            _ticketManager = ticketManager;
        }

        public Task<Result<QueryResult>> Handle(Command command, CancellationToken cancellationToken)
        {
            try
            {
                var timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(command.TimeZoneId);
                
                if (timeZone == null)
                {
                    return Task.FromResult(Result.Fail<QueryResult>("Неверный часовой пояс."));
                }
                
                var fromUtc = DateTimeHelper.ConvertToUtc(command.From, command.TimeZoneId);
                var toUtc = DateTimeHelper.ConvertToUtc(command.To, command.TimeZoneId);
                
                var tickets = _ticketManager.GetTickets();
                
                var filteredTickets = tickets
                    .Where(ticket => ticket.VisitDate >= fromUtc && ticket.VisitDate <= toUtc)
                    .ToList();
                
                var ticketsWithLocalTime = filteredTickets.Select(ticket => new TicketViewModel
                {
                    ID = ticket.ID,
                    Title = ticket.Title,
                    Description = ticket.Description,
                    VisitDate = DateTimeHelper.ConvertToLocal(ticket.VisitDate, command.TimeZoneId), 
                    VisitorsNumber = ticket.VisitorsNumber
                    
                }).ToList();
                
                return Task.FromResult(Result.Ok(new QueryResult(ticketsWithLocalTime)));
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occurred in GetTickets. reason={reason}", e.Message);
                return Task.FromResult<Result<QueryResult>>(Result.Fail("Не удалось получить заявки!"));
            }
        }
    }
}