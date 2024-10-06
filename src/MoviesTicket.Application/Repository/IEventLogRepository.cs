namespace MoviesTicket.Application.Repository;

public interface IEventLogRepository : IRepository<EventLogs>
{
    public Task<EventLogs> AddAsync(EventLogs eventLogs, CancellationToken cancellationToken);

}

