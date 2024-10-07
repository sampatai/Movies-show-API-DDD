using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoviesTicket.Application.CommandQuery.Command.Movie;
using MoviesTicket.Application.CommandQuery.Extension;
using MoviesTicket.Application.CommandQuery.Query;
using MoviesTicket.Application.Model;
using MoviesTicket.Application.Projections;
using System.Net;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MoviesTicket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController(ISender sender,
        ILogger<MovieController> logger) : ControllerBase
    {
        // GET: api/<MovieController>

        [HttpPost("movies")]
        [ProducesResponseType(typeof(ListMovie), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> FilterMovies([FromBody] FilterModel filterModel, CancellationToken cancellationToken)
        {
            try
            {
                var query = new GetMovies.Query
                {
                    Title = filterModel.Title,
                    ReleaseDate = filterModel.ReleaseDate,
                    Genres = filterModel.Genres,
                    ShowTime = filterModel.ShowTime,
                    PageNumber = filterModel.PageNumber,
                    PageSize = filterModel.PageSize
                };

                var result = await sender.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "@{filterModel}", filterModel);
                throw;

            }
        }

        [HttpGet("movie/{movieGuid:guid}")]
        [ProducesResponseType(typeof(GetMovie), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetAsync(Guid movieGuid, CancellationToken cancellationToken)
        {
            try
            {
                var result = await sender.Send(new GetMovieByGUID.Query(movieGuid), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching movie with GUID: {MovieGuid}", movieGuid);
                throw;
            }
        }

        [HttpPost()]
        [ProducesResponseType(typeof(CreateMovie), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Create([FromBody] CreateMovie create, CancellationToken cancellationToken)
        {
            try
            {
                await sender.Send(create.ToCreate(), cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "@{create}", create);
                throw;

            }
        }

        [HttpPut("{movieGuid:guid}")]
        [ProducesResponseType(typeof(UpdateMovie), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Update(Guid movieGuid, [FromBody] UpdateMovie update, CancellationToken cancellationToken)
        {
            try
            {
                var command = update.ToUpdate();
                command.MovieGUID = movieGuid;
                await sender.Send(command, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "@{update}", update);
                throw;

            }
        }

        [HttpPost("{movieGuid:guid}/shows")]
        [ProducesResponseType(typeof(CreateMovieShowTime), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> CreateShowsTime(Guid movieGuid, [FromBody] CreateMovieShowTime showTime, CancellationToken cancellationToken)
        {
            try
            {
                var command = showTime.ToCreateShows();
                command.MovieGUID = movieGuid;
                await sender.Send(command, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "@{showTime}", showTime);
                throw;

            }
        }
        [HttpPut("{movieGuid:guid}/shows")]
        [ProducesResponseType(typeof(UpdateMovieShowTime), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateShowsTime(Guid movieGuid, [FromBody] UpdateMovieShowTime update, CancellationToken cancellationToken)
        {
            try
            {
                var command = update.ToUpdateShows();
                command.MovieGUID = movieGuid;
                await sender.Send(command, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "@{update}", update);
                throw;

            }
        }
    }
}
