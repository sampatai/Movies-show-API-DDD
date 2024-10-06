
namespace MoviesTicket.Domain.Aggregates.Events;

public record ShowTimeAddedEvent(Guid ShowTimeGuid, string Title, DateTime ShowDate, string showTime) : INotification { }
public record ShowTimeUpdatedEvent(Guid ShowTimeGuid, string OldTitle, string NewTitle, DateTime ShowDate) : INotification { }
public record ShowTimeDeletedEvent(Guid ShowTimeGuid, string Title, DateTime ShowDate, string showTime) : INotification { }

public record ShowBookedEvent(Guid ShowTimeGuid, string Title, DateTime ShowDate, string showTime) : INotification { }