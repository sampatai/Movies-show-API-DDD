using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesTicket.Application.DomainEvents
{
  
     public class ShowTimeEventHandler : INotificationHandler<ShowTimeAddedEvent>
    {
        private readonly ILogger<ShowTimeEventHandler> _logger;
        private readonly IEventLogRepository _eventLogRepository;
        public ShowTimeEventHandler(ILoggerFactory logger,
        IEventLogRepository eventLogRepository)
        {
            _logger = logger.CreateLogger<ShowTimeEventHandler>();
            _eventLogRepository = eventLogRepository;
        }

        public async Task Handle(ShowTimeAddedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"movie: {notification.MovieGuid} has been created.");

            try
            {
                var description = $"Show timeAdded:New show time {notification.ShowTime} of  movie {notification.Title} added on {notification.ShowDate.ToShortDateString()}";
                EventLogs eventLog = new(description, EventType.ShowTimeAdded);
                await _eventLogRepository.AddAsync(eventLog, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{@notification}", notification);
                throw;
            }
        }
    }
     public class ShowTimeUpdatedEventHandler : INotificationHandler<ShowTimeUpdatedEvent>
    {
        private readonly ILogger<ShowTimeUpdatedEventHandler> _logger;
        private readonly IEventLogRepository _eventLogRepository;
        public ShowTimeUpdatedEventHandler(ILoggerFactory logger,
        IEventLogRepository eventLogRepository)
        {
            _logger = logger.CreateLogger<ShowTimeUpdatedEventHandler>();
            _eventLogRepository = eventLogRepository;
        }

        public async Task Handle(ShowTimeUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"movie: {notification.ShowTimeGuid} has been created.");

            try
            {
                var description = $"Show Time Updated: show time updated {notification.OldShowTime} to {notification.ShowTime} added on {notification.ShowDate.ToShortDateString()}";
                EventLogs eventLog = new(description, EventType.ShowTimeUpdated);
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