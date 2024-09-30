using MoviesTicket.Domain.Aggregates.ValueObjects;

namespace MoviesTicket.Domain.Aggregates.Entities;
public class ShowsTime : Entity
{
    private List<Reservation> _reservations = new();
    protected ShowsTime()
    {

    }
    public DateTime ShowDate { get; private set; }
    public string Time { get; private set; }
    public bool IsDeleted { get; private set; }
    public IReadOnlyList<Reservation> Reservation => _reservations.AsReadOnly();
  
}

