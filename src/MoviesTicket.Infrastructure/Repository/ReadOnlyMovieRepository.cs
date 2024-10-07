using MoviesTicket.Application.Projections;
using MoviesTicket.Domain.Aggregates.Enumerations;
using MoviesTicket.Infrastructure.Extensions;
namespace MoviesTicket.Infrastructure.Repository;

public class ReadOnlyMovieRepository(MoviesTicketDbContext moviesTicketDbContext,
    ILogger<ReadOnlyMovieRepository> logger) : IReadOnlyMovieRepository
{
    public async Task<(IEnumerable<Movies> Movies, int TotalCount)> GetMovies(FilterModel searchModel, CancellationToken cancellationToken)
    {
        try
        {
            var query = moviesTicketDbContext.Movies
            .AsQueryable();


            query = query.WhereIf(!string.IsNullOrEmpty(searchModel.Title), m => m.Title.Contains(searchModel.Title, StringComparison.OrdinalIgnoreCase));
            query = query.WhereIf(searchModel.ReleaseDate.HasValue, m => m.ReleaseDate.Date == searchModel.ReleaseDate!.Value.Date);
            query = query.WhereIf(searchModel.Genres is not null && Enumeration.GetAll<MovieGenres>().Any(x => x.Id == searchModel.Genres.Id), m => m.Genres == searchModel.Genres);
            query = query.WhereIf(!string.IsNullOrEmpty(searchModel.ShowTime), m => m.ShowsTimes.Any(st => st.Time.Contains(searchModel.ShowTime, StringComparison.OrdinalIgnoreCase)));

            var totalCount = await query.AsNoTracking().CountAsync(cancellationToken);

            var movies = await query.AsNoTracking()
                .Skip((searchModel.PageNumber - 1) * searchModel.PageSize)
                .Take(searchModel.PageSize)
                .ToListAsync(cancellationToken);
            return (movies, totalCount);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "@{searchModel}", searchModel);
            throw;
        }
    }
    public async Task<bool> HasMovies(Guid movieGuid, CancellationToken cancellationToken)
    {
        try
        {
            return await moviesTicketDbContext
                        .Movies
                        .AsNoTracking()
                        .AnyAsync(x => x.MovieGUID.Equals(movieGuid)
                                && x.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{@movieGuid}", movieGuid);
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
                  .AsNoTracking()
                  .FirstOrDefaultAsync(x => x.MovieGUID.Equals(movieGuid), cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "@{movieGuid}", movieGuid);
            throw;
        }
    }
}

