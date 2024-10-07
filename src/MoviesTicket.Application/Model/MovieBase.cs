public record MovieBase
{
     public  string Title { get;  set; }

    public DateTime ReleaseDate { get;  set; }

    public  MovieGenres Genres { get;  set; }

    public  string Runtime { get;  set; }

    public  string Director { get;  set; }

    public  string Synopsis { get;  set; }
}
public interface IMovieGUIDBase
{
 public Guid MovieGUID{get;set;}
}
public record CreateMovie:MovieBase
{

}
public record UpdateMovie : MovieBase, IMovieGUIDBase
{
    public Guid MovieGUID { get ; set;}
}
public record MovieResponse :MovieBase, IMovieGUIDBase
{
    public Guid MovieGUID { get ; set;}
}
public record ListMovie(IEnumerable<MovieResponse> Movies,int TotalCount);