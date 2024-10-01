namespace MoviesTicket.Domain.Aggregates.Enumerations;
public class EventType : Enumeration
{
    public static EventType Added = new(1, nameof(Added));
    public EventType(int id, string name) : base(id, name)
    {
    }
}

