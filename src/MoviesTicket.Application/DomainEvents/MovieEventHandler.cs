namespace MoviesTicket.Application.DomainEvents
{
    public class MovieAddedEventHandler : INotificationHandler<MovieAddedEvent>
    {
        private readonly ILogger<MovieAddedEventHandler> _logger;
        private readonly IEventLogRepository _eventLogRepository;
        public MovieAddedEventHandler(ILoggerFactory logger,
        IEventLogRepository eventLogRepository)
        {
            _logger = logger.CreateLogger<MovieAddedEventHandler>();
            _eventLogRepository = eventLogRepository;
        }

        public async Task Handle(MovieAddedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"movie: {notification.MovieGuid} has been created.");

            try
            {
                var description = $"Added:New movie {notification.Title} added of {notification.MovieGenres.Name}";
                EventLogs eventLog = new(description, EventType.Added);
                await _eventLogRepository.AddAsync(eventLog, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{@notification}", notification);
                throw;
            }
        }
    }
    public class MovieUpdatedEventEventHandler : INotificationHandler<MovieUpdatedEvent>
    {
        private readonly ILogger<MovieUpdatedEventEventHandler> _logger;
        private readonly IEventLogRepository _eventLogRepository;
        public MovieUpdatedEventEventHandler(ILoggerFactory logger,
        IEventLogRepository eventLogRepository)
        {
            _logger = logger.CreateLogger<MovieUpdatedEventEventHandler>();
            _eventLogRepository = eventLogRepository;
        }

        public async Task Handle(MovieUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"movie: {notification.MovieGuid} has been created.");

            try
            {
                var description = $"Updated: {notification.OldTitle} to  {notification.NewTitle}";
                EventLogs eventLog = new(description, EventType.Updated);
                await _eventLogRepository.AddAsync(eventLog, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{@notification}", notification);
                throw;
            }
        }
    }
}