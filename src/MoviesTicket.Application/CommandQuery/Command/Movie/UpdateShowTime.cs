using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoviesTicket.Application.Model;
using MoviesTicket.Application.Repository;

namespace MoviesTicket.Application.CommandQuery.Command.Movie
{
    public static class UpdateShowTimeCommand
    {
        #region Command/Query
        public sealed record Command : UpdateMovieShowTime, IRequest { }
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
                            System.Text.RegularExpressions.Regex.IsMatch(time.Time, @"^(?:\d|2[0-3]):[0-5]\d$") &&
                            time.MovieShowTimeGUID != Guid.Empty))
                        .WithMessage("Each MovieShowTime must have a valid Time in the format HH:mm and a valid MovieShowTimeGUID.");
            }
        }
        #endregion
    }
}