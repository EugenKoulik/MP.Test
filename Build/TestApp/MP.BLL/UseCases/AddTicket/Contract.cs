using FluentResults;
using MediatR;
using MP.Model.WS;

namespace MP.BLL.UseCases.AddTicket;

public static partial class AddTicketUseCase
{
    public record Command(TicketSaveModel Model, string TimeZoneId) : IRequest<Result<QueryResult>>;
    
    public record QueryResult(long Id);
}