namespace MoviesTicket.Domain.Aggregates.Events;

public record MovieAddedEvent(Guid MovieGuid, string Title, MovieGenres MovieGenres) : INotification { }
public record MovieUpdatedEvent(Guid MovieGuid, string OldTitle, string NewTitle, MovieGenres MovieGenres) : INotification { }
public record MovieInactiveEvent(Guid MovieGuid, string Title) : INotification { }
