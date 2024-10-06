namespace MoviesTicket.Application.Projections;

public class FilterModel
{
    public string Title { get; set; } = string.Empty;
    public DateTime? ReleaseDate { get; set; } = null;
    public MovieGenres Genres { get; set; }
    public string ShowTime { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}