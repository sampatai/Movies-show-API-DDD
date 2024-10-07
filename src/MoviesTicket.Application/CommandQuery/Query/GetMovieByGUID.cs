
namespace MoviesTicket.Application.CommandQuery.Query
{
    public static class GetMovieByGUID
    {
        #region Request
        public record Query(Guid MovieGUID) : IRequest<GetMovie>
        {

        }
        #endregion
        #region Validation
        protected sealed class Validator : AbstractValidator<Query>
        {

            public Validator(IReadOnlyMovieRepository readOnlyMovieRepository)
            {
                RuleFor(x => x.MovieGUID).MustAsync(readOnlyMovieRepository.HasMovies)
                                         .WithMessage("Invalid movie name");
            }

        }

        #endregion

        #region Handler
        protected sealed class Handler(ILogger<Handler> logger,
                IReadOnlyMovieRepository readOnlyMovieRepository) : IRequestHandler<Query, GetMovie>
        {
            public async Task<GetMovie> Handle(Query request, CancellationToken cancellationToken)
            {

                try
                {
                    var result = await readOnlyMovieRepository.GetAsync(request.MovieGUID, cancellationToken);
                    return result.ToMovie();
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
}