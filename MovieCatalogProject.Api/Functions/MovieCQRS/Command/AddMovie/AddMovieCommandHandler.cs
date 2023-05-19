using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MovieCatalogProject.Domain.Common;
using MovieCatalogProject.Domain.Entities;
using MovieCatalogProject.Infrastructure.Repositories;

namespace MovieCatalogProject.Api.Functions.MovieCQRS.Command.AddMovie
{
    public record AddMovieCommand(string name, DateTime releaseDate, string[] genres) : IRequest;
    public class AddMovieCommandHandler : IRequestHandler<AddMovieCommand>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Movie> _movieRepository;
        ILogger<AddMovieCommandHandler> _logger;

        public AddMovieCommandHandler(IMapper mapper, IGenericRepository<Movie> movieRepository, ILogger<AddMovieCommandHandler> logger)
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
            _logger = logger;
        }

        public async Task Handle(AddMovieCommand request, CancellationToken cancellationToken)
        {

            var movie = _mapper.Map<Movie>(request);
            await _movieRepository.AddAsync(movie);
            _logger.LogWarning($"Entity with id:{movie.Id} INSERT action invoke");
        }
    }

}
