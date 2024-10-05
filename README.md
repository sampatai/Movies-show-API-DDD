# Movies-show-API-DDD

This demo project primarily implements domain-driven design (DDD) concepts. It follows the principles of clean architecture to ensure a clear separation of concerns and maintainability. The project structure is organized to promote scalability and testability, making it easier to manage and extend.
## About

The Movies Ticketing System is designed to manage movie reservations, showtimes, and genres. It demonstrates key concepts of DDD, including entities, value objects, aggregates, repositories, and services. It follows the SOLID principle, CQRS pattern, implement unit test for domain and application layers.
## Key Features:
- **Domain-Driven Design (DDD)**: The core business logic is encapsulated within the domain layer, ensuring that the business rules are the primary focus.
- **Clean Architecture**: The project is structured to separate the application into distinct layers, including the domain, application, infrastructure, and presentation layers. This separation helps in managing dependencies and promotes a clean, maintainable codebase.
- **Entity Framework Core**: Used for data access, with configurations done using Fluent API to ensure precise control over the database schema.
- **Unit Testing**: Comprehensive unit tests are provided to ensure the reliability and correctness of the business logic.
  
## Domain Layer

- **Entities**: These are the core objects of the domain model, representing the main business objects. For example, in the `MoviesTicket.Domain` project, entities like `Movies`, `ShowsTime` encapsulate the business logic and rules.
  ### Example Usage
 ```csharp
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
```


- **Value Objects**: These are objects that describe some characteristics or attributes but do not have a distinct identity. For instance, `Reservation` is a value object.
 ### Example Usage
 ```csharp
using Ardalis.GuardClauses;

namespace MoviesTicket.Domain.Aggregates.ValueObjects;
public class Reservation : ValueObject
{
    protected Reservation()
    {

    }
    public Reservation(string firstName, string lastName, string email)
    {
        this.FirstName = Guard.Against.NullOrEmpty(firstName);
        this.LastName = Guard.Against.NullOrEmpty(lastName);
        this.Email = Guard.Against.NullOrEmpty(email);
    }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }

    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return Email;
    }
}
```

- **Aggregates and Aggregate Roots**: Aggregates are clusters of domain objects that can be treated as a single unit. The aggregate root is the main entity that holds references to other entities. In this project, `Movies` is an aggregate root that manages `ShowsTime` and `Reservation`.
 ### Example Usage
 ```csharp
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
```
