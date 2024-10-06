namespace MoviesTicket.Application.Repository;

public interface IMovieRepository : IRepository<Movies>
{
    public Task<Movies> AddAsync(Movies movies, CancellationToken cancellationToken);
    public Task<Movies> UpdateAsync(Movies movies, CancellationToken cancellationToken);
    public Task<Movies> GetAsync(Guid movieGuid, CancellationToken cancellationToken);
}

