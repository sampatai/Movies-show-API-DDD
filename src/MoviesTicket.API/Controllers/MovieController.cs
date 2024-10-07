using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoviesTicket.Application.CommandQuery.Query;
using MoviesTicket.Application.Projections;
using MoviesTicket.Domain.Aggregates.Root;
using System.Net;
using System.Threading;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MoviesTicket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController(ISender sender,
        ILogger<MovieController> logger) : ControllerBase
    {
        // GET: api/<MovieController>

        [HttpPost]
        [ProducesResponseType(typeof(ListMovie), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetAsync([FromBody] FilterModel filterModel, CancellationToken cancellationToken)
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


    }
}
