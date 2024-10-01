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
        this.Title = Guard.Against.NullOrEmpty(title);
        this.ReleaseDate = Guard.Against.Null(releaseDate);
        this.Genres = Guard.Against.Null(movieGenres);
        this.Runtime = Guard.Against.NullOrEmpty(runtime);
        this.Director = Guard.Against.NullOrEmpty(Director);
        this.Synopsis = Guard.Against.NullOrEmpty(synopsis);
    }

    public void SetInactive()
    {
        this.IsActive = false;
    }
    public void AddShowsTimes(IEnumerable<ShowsTime> showsTimes)
    {
        _showsTimes.AddRange(showsTimes);
    }
    public void SetShowTime(Guid showTimeGuid, DateTime showDate,
        string time)
    {
        var single = _showsTimes
            .Where(x => x.ShowsTimeGUID == showTimeGuid)
            .SingleOrDefault()!;
        single.SetShowsTime(showDate, time);
    }
    public void AddReservation(Guid showTimeGuid, Reservation reservation)
    {
        var single = _showsTimes
            .Where(x => x.ShowsTimeGUID == showTimeGuid)
            .SingleOrDefault()!;
        single.SetReservation(reservation);
    }

    public void DeleteReservation(Guid showTimeGuid, Reservation reservation)
    {
        var single = _showsTimes
           .Where(x => x.ShowsTimeGUID == showTimeGuid)
           .SingleOrDefault()!;
        single.RemoveReservation(reservation);
    }
}

