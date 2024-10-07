using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoviesTicket.Application.Model;

namespace MoviesTicket.Application.CommandQuery.Extension
{
    public static class MovieExtension
    {
        public static IEnumerable<MovieResponse> ToMovies(this IEnumerable<Movies> movies)
        {

            return movies.Select(x => new MovieResponse()
            {
                Title = x.Title,
                ReleaseDate = x.ReleaseDate,
                Genres = x.Genres,
                Runtime = x.Runtime,
                Director = x.Director,
                Synopsis = x.Synopsis,
                MovieGUID = x.MovieGUID
            });
        }

        public static GetMovie ToMovie(this Movies movie)
        {

            var response = new MovieResponse()
            {
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Genres = movie.Genres,
                Runtime = movie.Runtime,
                Director = movie.Director,
                Synopsis = movie.Synopsis,
                MovieGUID = movie.MovieGUID
            };
            var movieshows = movie.ShowsTimes.GroupBy(x => x.ShowDate)
            .Select(x => new GetMovieShow()
            {
                MovieGUID = movie.MovieGUID,
                ShowDate = x.Key,
                MovieShowTimes = x.Select(s => new MovieShowTimeGUIDBase()
                {
                    MovieShowTimeGUID = s.ShowsTimeGUID,
                    Time = s.Time
                })

            });
            return new GetMovie(response,movieshows);
        }
    }
}