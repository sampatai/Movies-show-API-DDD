

using MoviesTicket.Application.Repository;

namespace MoviesTicket.Application.CommandQuery.Command.Movie
{
    public static class UpdateMovieCommand
    {
        #region Command/Query
          public sealed record Command :UpdateMovie,IRequest{}
         #endregion

          #region Validation
          public sealed class Validator :Validator<Command>
          {
            public Validator(IReadOnlyMovieRepository readOnlyMovieRepository )
            {
                RuleFor(x => x.MovieGUID).MustAsync(readOnlyMovieRepository.HasMovies)
	                                     .WithMessage("Invalid movie name");
            }
          }
          #endregion
    }
}
