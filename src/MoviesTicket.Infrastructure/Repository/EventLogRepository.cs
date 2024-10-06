namespace MoviesTicket.Infrastructure.Repository;

public class EventLogRepository(MoviesTicketDbContext moviesTicketDbContext,
    ILogger<EventLogRepository> logger) : IEventLogRepository
{
    public IUnitOfWork UnitOfWork => moviesTicketDbContext;

    public async Task<EventLogs> AddAsync(EventLogs eventLogs, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await moviesTicketDbContext.EventLogs.AddAsync(eventLogs);
            return entity.Entity;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "@{eventLogs}", eventLogs);
            throw;
        }
    }
}

