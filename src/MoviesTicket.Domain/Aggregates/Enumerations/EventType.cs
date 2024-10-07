namespace MoviesTicket.Domain.Aggregates.Enumerations;
public class EventType : Enumeration
{
    public static EventType Added = new(1, nameof(Added));
    public static EventType Updated = new(2, nameof(Updated));
     public static EventType ShowTimeAdded = new(3, nameof(ShowTimeAdded));
     public static EventType ShowTimeUpdated = new(4, nameof(ShowTimeUpdated));
    public EventType(int id, string name) : base(id, name)
    {
    }
}

