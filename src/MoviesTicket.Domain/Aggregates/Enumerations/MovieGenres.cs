namespace MoviesTicket.Domain.Aggregates.Enumerations;
public class MovieGenres : Enumeration
{
    public static MovieGenres G = new(1, nameof(G));
    public static MovieGenres PG = new(2, nameof(PG));
    public static MovieGenres PG13 = new (3, "PG-13");
    public static MovieGenres R = new(4, "R");
    public static MovieGenres NC17 = new(5, "NC-17");
    public MovieGenres(int id, string name) : base(id, name)
    {
    }
}
