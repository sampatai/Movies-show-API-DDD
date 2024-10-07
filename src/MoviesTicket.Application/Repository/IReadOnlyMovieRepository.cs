

namespace MoviesTicket.Application.Repository;

public interface IReadOnlyMovieRepository : IReadOnlyRepository<Movies>
{
    public Task<(IEnumerable<Movies> Movies, int TotalCount)> GetMovies(FilterModel searchModel,CancellationToken cancellationToken);
    public Task<bool> HasMovies(Guid movieGuid,CancellationToken cancellationToken);
    Task<Movies> GetAsync(Guid movieGuid, CancellationToken cancellationToken);
}

