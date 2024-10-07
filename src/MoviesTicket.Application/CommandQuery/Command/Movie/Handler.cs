using MoviesTicket.Application.Repository;
using MoviesTicket.Domain.Aggregates.Entities;

namespace MoviesTicket.Application.CommandQuery.Command.Movie
{
    public sealed class Handler(ILogger<Handler> logger,
    IMovieRepository movieRepository) : IRequestHandler<CreateMovieCommand.Command>,
     IRequestHandler<UpdateMovieCommand.Command>,
     IRequestHandler<CreateShowTimeCommand.Command>,
     IRequestHandler<UpdateShowTimeCommand.Command>
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

        public async Task Handle(CreateShowTimeCommand.Command request, CancellationToken cancellationToken)
        {
            try
            {
                var singleMovie = await movieRepository.GetAsync(request.MovieGUID, cancellationToken);

                singleMovie.AddShowsTimes(request.MovieShowTimes.Select(x => new ShowsTime(request.ShowDate, x.Time)).ToList());

                await movieRepository.UpdateAsync(singleMovie, cancellationToken);
                await movieRepository.UnitOfWork.SaveEntitiesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{@request}", request);
                throw;
            }
        }

        public async   Task Handle(UpdateShowTimeCommand.Command request, CancellationToken cancellationToken)
        {
            try
            {
                var singleMovie = await movieRepository.GetAsync(request.MovieGUID, cancellationToken);

                foreach (var showTime in request.MovieShowTimes)
                {
                    singleMovie.SetShowTime(showTime.MovieShowTimeGUID, request.ShowDate, showTime.Time);
                }

                await movieRepository.UpdateAsync(singleMovie, cancellationToken);
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