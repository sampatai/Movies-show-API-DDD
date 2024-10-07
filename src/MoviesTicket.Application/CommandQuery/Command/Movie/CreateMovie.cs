namespace MoviesTicket.Application.CommandQuery.Command.Movie
{
    public static class CreateMovieCommand
    {
        #region Command/Query
        public sealed record Command : CreateMovie, IRequest { }
        #endregion

        #region Validation
        public sealed class Validator : Validator<Command>
        {
            public Validator()
            {

            }
        }
        #endregion
    }
}
