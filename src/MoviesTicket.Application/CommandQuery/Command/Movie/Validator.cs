namespace MoviesTicket.Application.CommandQuery.Command.Movie;

public abstract class Validator<T> : AbstractValidator<T>  where T :MovieBase
{
    public Validator()
    {
         RuleFor(movie => movie.Title)
            .NotEmpty().WithMessage("Title is required.");

        RuleFor(movie => movie.ReleaseDate)
            .NotEmpty().WithMessage("Release date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Release date cannot be in the past.");

        RuleFor(x => x.Genres)
                 .Must(x => Enumeration.GetAll<MovieGenres>().Any(a => a.Id == x.Id))               
                .WithMessage("Invalid genre.");

        RuleFor(movie => movie.Runtime)
            .NotEmpty().WithMessage("Runtime is required.")
            .Matches(@"^\d+ min$").WithMessage("Runtime should be in the format 'xxx min'.");

        RuleFor(movie => movie.Director)
            .NotEmpty().WithMessage("Director is required.");

        RuleFor(movie => movie.Synopsis)
            .NotEmpty().WithMessage("Synopsis is required.")
            .MaximumLength(1000).WithMessage("Synopsis cannot be longer than 1000 characters.");
    }
}
