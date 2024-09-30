namespace MoviesTicket.Domain.Aggregates.ValueObjects;
public class Reservation : ValueObject
{
    protected Reservation()
    {

    }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? Email { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return Email;
    }
}

