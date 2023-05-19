using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogProject.Api.Functions.MovieCQRS.Command.AddMovie;
using MovieCatalogProject.Api.Functions.MovieCQRS.Command.DeleteMovie;
using MovieCatalogProject.Api.Functions.MovieCQRS.Query.GetLastMovie;
using MovieCatalogProject.Api.Functions.MovieCQRS.Query.GetMoviesByGenre;
using MovieCatalogProject.Api.Functions.MovieCQRS.Query.GetMoviesByYear;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieCatalogProject.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<AddMovieCommand> _validator;
        public MovieController(IMediator mediator, IValidator<AddMovieCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpGet("movie")]
        public async Task<ActionResult> GetLastMovie()
        {
            var result = await _mediator.Send(new GetLastMovieQuery());
            return StatusCode(200, result);
        }

        [HttpGet("movie/year/{year}")]
        public async Task<ActionResult> GetMoviesByYear([FromRoute] int year)
        {
            
            var result = await _mediator.Send(new GetMoviesByYearQuery(year));
            return StatusCode(200, result);
        }

        [HttpGet("movie/genre/{genre}")]
        public async Task<ActionResult> GetMoviesByGenre([FromRoute] string genre)
        {
            var result = await _mediator.Send(new GetMoviesByGenreQuery(genre));
            return StatusCode(200, result);
        }
        [HttpPost("movie")]
        public async Task<ActionResult> AddMovie([FromBody] AddMovieCommand dto)
        {
            var validation = await _validator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }
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
