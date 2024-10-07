using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesTicket.Application.CommandQuery.Extension
{
    public static class MovieExtension
    {
        public static IEnumerable<MovieResponse> ToMovie(this IEnumerable<Movies>  movies ){

            return movies.Select(x=>new MovieResponse() {
                Title=x.Title,
                ReleaseDate=x.ReleaseDate,
                Genres=x.Genres,
                Runtime=x.Runtime,
                Director=x.Director,
                Synopsis=x.Synopsis,
                MovieGUID=x.MovieGUID
            });
        }
    }   
}