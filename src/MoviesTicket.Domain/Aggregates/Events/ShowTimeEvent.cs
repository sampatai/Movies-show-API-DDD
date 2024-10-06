
namespace MoviesTicket.Domain.Aggregates.Events;

public record ShowTimeAddedEvent(Guid MovieGuid, string Title, DateTime ShowDate, string ShowTime) : INotification { }
public record ShowTimeUpdatedEvent(Guid ShowTimeGuid, string OldShowTime, string ShowTime, DateTime ShowDate) : INotification { }
public record ShowTimeDeletedEvent(Guid ShowTimeGuid, string Title, DateTime ShowDate, string showTime) : INotification { }

public record ShowBookedEvent(Guid ShowTimeGuid, string Title, DateTime ShowDate, string showTime, string bookedBy) : INotification { }