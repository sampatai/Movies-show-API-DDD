using MoviesTicket.Domain.Aggregates.Root;

namespace MoviesTicket.Infrastructure.Repository;

internal class MovieRepository(MoviesTicketDbContext moviesTicketDbContext,
    ILogger<MovieRepository> logger) : IMovieRepository
{
    public IUnitOfWork UnitOfWork => moviesTicketDbContext;

    public async Task<Movies> AddAsync(Movies movies, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await moviesTicketDbContext.AddAsync(movies);
            return entity.Entity;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "@{movies}", movies);
            throw;
        }
    }

    public async Task<Movies> GetAsync(Guid movieGuid, CancellationToken cancellationToken)
    {
        try
        {
            return await moviesTicketDbContext
                  .Movies
                  .Include(x => x.ShowsTimes)
                  .ThenInclude(x => x.Reservation)
                  .FirstOrDefaultAsync(x => x.MovieGUID.Equals(movieGuid), cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "@{movieGuid}", movieGuid);
            throw;
        }
    }

    public async Task<Movies> UpdateAsync(Movies movies, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await Task.FromResult(moviesTicketDbContext.Update(movies));
            return entity.Entity;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "@{movies}", movies);
            throw;
        }
    }
}

