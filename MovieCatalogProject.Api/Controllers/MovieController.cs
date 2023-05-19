using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogProject.Api.Functions.MovieCQRS.Command;
using MovieCatalogProject.Api.Functions.MovieCQRS.Query;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieCatalogProject.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MovieController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("movie")]
        public async Task<ActionResult> GetLastMovie()
        {
            var result = await _mediator.Send(new GetLastMovieQuery());
            return StatusCode(200, result);
        }

        [HttpGet("movies/year/{year}")]
        public async Task<ActionResult> GetMoviesByYear([FromRoute] int year)
        {
            
            var result = await _mediator.Send(new GetMoviesByYearQuery(year));
            return StatusCode(200, result);
        }

        [HttpGet("movies/genre/{genre}")]
        public async Task<ActionResult> GetMoviesByGenre([FromRoute] string genre)
        {
            var result = await _mediator.Send(new GetMoviesByGenreQuery(genre));
            return StatusCode(200, result);
        }
        [HttpPost("movie")]
        public async Task<ActionResult> AddMovie([FromBody] AddMovieCommand dto)
        {
            await _mediator.Send(dto);
            return StatusCode(201);
        }

        [HttpDelete("movie/{id}")]
        public async Task<ActionResult> DeleteMovie(Guid id)
        {
            await _mediator.Send(new DeleteMovieCommand(id));

            return StatusCode(200);
        }
    }
}
