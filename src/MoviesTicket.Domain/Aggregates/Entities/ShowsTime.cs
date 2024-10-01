namespace MoviesTicket.Domain.Aggregates.Entities;
public class ShowsTime : Entity
{
    private List<Reservation> _reservations = new();
    protected ShowsTime()
    {

    }
    public ShowsTime(DateTime showDate,
        string time
        )
    {
        this.ShowDate = Guard.Against.Null(showDate);
        this.Time = Guard.Against.NullOrEmpty(time);
        IsDeleted = false;
        ShowsTimeGUID = Guid.NewGuid();
    }
    public Guid ShowsTimeGUID { get; private set; }
    public DateTime ShowDate { get; private set; }
    public string Time { get; private set; }
    public bool IsDeleted { get; private set; }
    public IReadOnlyList<Reservation> Reservation => _reservations.AsReadOnly();

    internal void SetReservation(Reservation reservation)
    {
        _reservations.Add(reservation);
    }
    internal void SetShowsTime(DateTime showDate,
        string time)
    {
        this.ShowDate = Guard.Against.Null(showDate);
        this.Time = Guard.Against.NullOrEmpty(time);
    }
    internal void DeleteShow()
    {
        this.IsDeleted = true;
        _reservations.Clear();
    }
    internal void RemoveReservation(Reservation reservation)
    {
        _reservations
            .RemoveAll(x => x.FirstName.Equals(reservation.FirstName)
            && x.LastName.Equals(reservation.LastName)
            && x.Email.Equals(reservation.Email));
    }
}

