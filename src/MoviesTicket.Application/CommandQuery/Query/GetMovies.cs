using FluentValidation;
using MoviesTicket.Application.Projections;
using MoviesTicket.Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MoviesTicket.Application.CommandQuery.Query;

public static class GetMovies
{
    public record Query : FilterModel, IRequest<IEnumerable<Movies>>
    {

    }

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



    protected sealed class Handler(ILogger<Handler> logger,
            IReadOnlyMovieRepository readOnlyMovieRepository) : IRequestHandler<Query, IEnumerable<Movies>>
    {
        public async Task<IEnumerable<Movies>> Handle(Query request, CancellationToken cancellationToken)
        {

            try
            {
                return (await readOnlyMovieRepository.GetMovies(request, cancellationToken)).Movies;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "@{request}", request);
                throw;
            }
        }
    }
}

