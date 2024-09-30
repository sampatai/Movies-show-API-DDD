using MoviesTicket.Domain.Aggregates.Entities;
using MoviesTicket.Domain.Aggregates.Enumerations;

namespace MoviesTicket.Domain.Aggregates.Root;

public class Movies : Entity, IAggregateRoot
{
    private List<ShowsTime> _showsTimes = new();
    protected Movies()
    {

    }

    public string Title { get; private set; }

    public DateTime ReleaseDate { get; private set; }

    public MovieGenres Genres { get; private set; }

    public string Runtime { get; private set; }

    public string Director { get; private set; }

    public string Synopsis { get; private set; }

    public IReadOnlyList<ShowsTime> ShowsTimes => _showsTimes.AsReadOnly();




}

