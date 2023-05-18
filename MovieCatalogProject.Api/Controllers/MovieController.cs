﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogProject.Api.Functions.MovieCQRS.Command;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieCatalogProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MovieController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/<MovieController>
        [HttpPost]
        public async Task<ActionResult> AddMovie([FromBody] AddMovieCommand dto)
        {
            await _mediator.Send(dto);
            return StatusCode(201);
        }

        // DELETE api/<MovieController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMovie(Guid id)
        {
            await _mediator.Send(new DeleteMovieCommand(id));

            return StatusCode(200);
        }
    }
}