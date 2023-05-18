using AutoMapper;
using MediatR;
using MovieCatalogProject.Domain.Common;
using MovieCatalogProject.Domain.Entities;

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
            try
            {
                var movie = await _movieRepository.GetByIdAsync(request.id);
                if (movie == null)
                    throw new NullReferenceException($"there is no movie with id {request.id}");
                _movieRepository.Delete(movie);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
