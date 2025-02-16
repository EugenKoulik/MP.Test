using System.Collections.Generic;
using MP.Domain.Model;

namespace MP.Domain.Object
{
    public interface IManager
    {
        List<Ticket> GetTickets();

        void AddTicket(Ticket ticket);
    }
}
