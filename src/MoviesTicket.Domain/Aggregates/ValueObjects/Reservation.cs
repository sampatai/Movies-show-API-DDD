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

