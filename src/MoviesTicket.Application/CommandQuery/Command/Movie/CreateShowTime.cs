using MoviesTicket.Application.Model;
using MoviesTicket.Application.Repository;

namespace MoviesTicket.Application.CommandQuery.Command.Movie
{
    public static class CreateShowTimeCommand
    {
        #region Command/Query
        public sealed record Command : CreateMovieShowTime, IRequest
        {
            public  Guid MovieGUID { get; set; }

        }
        #endregion
        #region Validation
        public sealed class Validator : MovieShowValidator<Command>
        {
            public Validator(IReadOnlyMovieRepository readOnlyMovieRepository) : base(readOnlyMovieRepository)
            {
                RuleFor(show => show.MovieShowTimes)
                         .NotEmpty().WithMessage("MovieShowTimes are required.")
                         .Must(times => times != null && times.All(time =>
                             !string.IsNullOrEmpty(time.Time) &&
                             System.Text.RegularExpressions.Regex.IsMatch(time.Time, @"^(?:\d|2[0-3]):[0-5]\d$")))
                         .WithMessage("Each MovieShowTime must have a valid Time in the format HH:mm.");
            }
        }
        #endregion
    }
}