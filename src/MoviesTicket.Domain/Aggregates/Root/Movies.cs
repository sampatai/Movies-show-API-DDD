using MoviesTicket.Domain.Aggregates.Events;

namespace MoviesTicket.Domain.Aggregates.Root;

public class Movies : Entity, IAggregateRoot
{
    private List<ShowsTime> _showsTimes = new();
    protected Movies()
    {

    }

    public Movies(string title,
       DateTime releaseDate,
       MovieGenres movieGenres,
      string runtime,
      string director,
      string synopsis)
    {
        this.Title = Guard.Against.NullOrEmpty(title);
        this.ReleaseDate = Guard.Against.Null(releaseDate);
        this.Genres = Guard.Against.Null(movieGenres);
        this.Runtime = Guard.Against.NullOrEmpty(runtime);
        this.Director = Guard.Against.NullOrEmpty(director);
        this.Synopsis = Guard.Against.NullOrEmpty(synopsis);
        this.MovieGUID = Guid.NewGuid();
        this.IsActive = true;
        AddDomainEvent(new MovieAddedEvent(MovieGUID, Title, Genres));
    }
    public Guid MovieGUID { get; private set; }
    public string Title { get; private set; }

    public DateTime ReleaseDate { get; private set; }

    public MovieGenres Genres { get; private set; }

    public string Runtime { get; private set; }

    public string Director { get; private set; }

    public string Synopsis { get; private set; }
    public bool IsActive { get; private set; }
    public IReadOnlyList<ShowsTime> ShowsTimes => _showsTimes.AsReadOnly();


    public void SetMovie(string title,
       DateTime releaseDate,
       MovieGenres movieGenres,
      string runtime,
      string director,
      string synopsis)
    {
        string oldTitle = Title;
        this.Title = Guard.Against.NullOrEmpty(title);
        this.ReleaseDate = Guard.Against.Null(releaseDate);
        this.Genres = Guard.Against.Null(movieGenres);
        this.Runtime = Guard.Against.NullOrEmpty(runtime);
        this.Director = Guard.Against.NullOrEmpty(director);
        this.Synopsis = Guard.Against.NullOrEmpty(synopsis);

        AddDomainEvent(new MovieUpdatedEvent(MovieGUID, oldTitle, Title, Genres));
    }

    public void SetInactive()
    {
        this.IsActive = false;
        AddDomainEvent(new MovieInactiveEvent(MovieGUID, Title));

    }
    public void AddShowsTimes(List<ShowsTime> showsTimes)
    {
        _showsTimes.AddRange(showsTimes);
        var domainEvent = showsTimes.GroupBy(x => x.ShowDate)
              .Select(x => new ShowTimeAddedEvent(MovieGUID, Title, x.Key, string.Join("", x.SelectMany(a => a.Time))));
        foreach (var t in domainEvent)
            AddDomainEvent(t);


    }
    public void SetShowTime(Guid showTimeGuid, DateTime showDate,
        string time)
    {
        var single = _showsTimes
            .Where(x => x.ShowsTimeGUID == showTimeGuid)
            .SingleOrDefault()!;
        string oldShowTime = single.Time;
        single.SetShowsTime(showDate, time);
        AddDomainEvent(new ShowTimeUpdatedEvent(showTimeGuid, oldShowTime, time, showDate));

    }
    public void AddReservation(Guid showTimeGuid, Reservation reservation)
    {
        var single = _showsTimes
            .Where(x => x.ShowsTimeGUID == showTimeGuid)
            .SingleOrDefault()!;
        single.SetReservation(reservation);
        AddDomainEvent(new ShowBookedEvent(showTimeGuid, Title, single.ShowDate, single.Time, $"{reservation.FirstName} {reservation.LastName}"));

    }

    public void DeleteReservation(Guid showTimeGuid, Reservation reservation)
    {
        var single = _showsTimes
           .Where(x => x.ShowsTimeGUID == showTimeGuid)
           .SingleOrDefault()!;
        single.RemoveReservation(reservation);
    }
}

