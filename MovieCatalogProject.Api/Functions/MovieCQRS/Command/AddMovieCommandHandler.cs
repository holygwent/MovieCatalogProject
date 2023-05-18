using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MovieCatalogProject.Domain.Common;
using MovieCatalogProject.Domain.Entities;

namespace MovieCatalogProject.Api.Functions.MovieCQRS.Command
{
    public record AddMovieCommand(string Name, DateTime ReleaseDate, string[] Genres) : IRequest;
    public class AddMovieCommandHandler : IRequestHandler<AddMovieCommand>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Movie> _movieRepository;

        public AddMovieCommandHandler(IMapper mapper, IGenericRepository<Movie> movieRepository)
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
        }

        public async Task Handle(AddMovieCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var movie = _mapper.Map<Movie>(request);
                await _movieRepository.AddAsync(movie);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

}
