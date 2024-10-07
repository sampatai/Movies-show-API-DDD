




namespace MoviesTicket.Application.CommandQuery.Query;

public static class GetMovies
{
    #region Request
    public record Query : FilterModel, IRequest<ListMovie>
    {

    }
    #endregion
    #region Validation
    protected sealed class Validator : AbstractValidator<Query>
    {
        public Validator()
        {

            RuleFor(x => x.Genres)
                 .Must(x => Enumeration.GetAll<MovieGenres>().Any(a => a.Id == x.Id))
                 .When(x => x.Genres is not null && x.Genres.Id > 0)
                .WithMessage("Invalid genre.");



            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
        }
    }

    #endregion
    #region Handler
    protected sealed class Handler(ILogger<Handler> logger,
            IReadOnlyMovieRepository readOnlyMovieRepository) : IRequestHandler<Query, ListMovie>
    {
        public async Task<ListMovie> Handle(Query request, CancellationToken cancellationToken)
        {

            try
            {
                var result = await readOnlyMovieRepository.GetMovies(request, cancellationToken);
                return new ListMovie(result.Movies.ToMovies(), result.TotalCount);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "@{request}", request);
                throw;
            }
        }
    }
    #endregion

}

