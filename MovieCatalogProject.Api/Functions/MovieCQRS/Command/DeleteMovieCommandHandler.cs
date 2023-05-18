using AutoMapper;
using MediatR;
using MovieCatalogProject.Domain.Common;
using MovieCatalogProject.Domain.Entities;
using MovieCatalogProject.Infrastructure.Exceptions;

namespace MovieCatalogProject.Api.Functions.MovieCQRS.Command
{
    public record DeleteMovieCommand(Guid id) : IRequest;
    public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand>
    {
        private readonly IGenericRepository<Movie> _movieRepository;

        public DeleteMovieCommandHandler(IGenericRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            
                _movieRepository.Delete(request.id);
              
            
        }
    }
}
