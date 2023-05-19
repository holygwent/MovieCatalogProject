using MediatR;
using MovieCatalogProject.Domain.Common;
using MovieCatalogProject.Domain.Entities;

namespace MovieCatalogProject.Api.Functions.MovieCQRS.Query
{
    public record GetMoviesByGenreQuery(string genre):IRequest<GetMoviesByGenreQueryResponse>;
    public class GetMoviesByGenreQueryHandler : IRequestHandler<GetMoviesByGenreQuery, GetMoviesByGenreQueryResponse>
    {
        private readonly IGenericRepository<Movie> _movieRepository;
        public GetMoviesByGenreQueryHandler(IGenericRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;
        }
        public async Task<GetMoviesByGenreQueryResponse> Handle(GetMoviesByGenreQuery request, CancellationToken cancellationToken)
        {
            var movies = _movieRepository.GetAllAsync().Result.Where(x=>x.Genres.Contains(request.genre)).ToList();
            return new GetMoviesByGenreQueryResponse(movies);
        }
    }
    public record GetMoviesByGenreQueryResponse(List<Movie> movies);
}
