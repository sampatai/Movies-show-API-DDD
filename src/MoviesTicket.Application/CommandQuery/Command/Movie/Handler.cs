using MoviesTicket.Application.Repository;

namespace MoviesTicket.Application.CommandQuery.Command.Movie
{
    public sealed class Handler(ILogger<Handler> logger,
    IMovieRepository movieRepository) : IRequestHandler<CreateMovieCommand.Command>, IRequestHandler<UpdateMovieCommand.Command>
    {
        public async Task Handle(UpdateMovieCommand.Command request, CancellationToken cancellationToken)
        {
            try
            {
                var singleMovie = await movieRepository.GetAsync(request.MovieGUID, cancellationToken);
                singleMovie.SetMovie(request.Title,
                                    request.ReleaseDate,
                                    request.Genres,
                                    request.Runtime,
                                    request.Director,
                                    request.Synopsis);


                await movieRepository.UpdateAsync(singleMovie, cancellationToken);
                await movieRepository.UnitOfWork.SaveEntitiesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{@request}", request);
                throw;
            }
        }

        public async Task Handle(CreateMovieCommand.Command request, CancellationToken cancellationToken)
        {
            try
            {
                var movie = new Movies(request.Title,
                                    request.ReleaseDate,
                                    request.Genres,
                                    request.Runtime,
                                    request.Director,
                                    request.Synopsis);

                await movieRepository.AddAsync(movie, cancellationToken);
                await movieRepository.UnitOfWork.SaveEntitiesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{@request}", request);
                throw;
            }
        }
    }
}