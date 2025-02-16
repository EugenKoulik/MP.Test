using FluentResults;
using MediatR;
using MP.Model.WS;

namespace MP.BLL.UseCases.GetTickets;

public static partial class GetTicketUseCase
{
    public record Command(DateTime From, DateTime To, string TimeZoneId) : IRequest<Result<QueryResult>>;
    
    public record QueryResult(IEnumerable<TicketViewModel> TicketViewModels);
}