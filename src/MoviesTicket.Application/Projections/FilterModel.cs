namespace MoviesTicket.Application.Projections;

public record FilterModel
{
    public string Title { get; set; } = string.Empty;
    public DateTime? ReleaseDate { get; set; } = null;
    public MovieGenres? Genres { get; set; } = new MovieGenres(0, "");
    public string ShowTime { get; set; } = string.Empty;
    public required int PageNumber { get; set; } = 1;
    public required int PageSize { get; set; } = 10;
}