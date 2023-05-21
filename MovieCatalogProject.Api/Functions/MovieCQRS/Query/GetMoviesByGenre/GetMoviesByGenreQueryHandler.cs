using MediatR;
using MovieCatalogProject.Domain.Common;
using MovieCatalogProject.Domain.Entities;

namespace MovieCatalogProject.Api.Functions.MovieCQRS.Query.GetMoviesByGenre
{
    public record GetMoviesByGenreQuery(string genre) : IRequest<GetMoviesByGenreQueryResponse>;
    public class GetMoviesByGenreQueryHandler : IRequestHandler<GetMoviesByGenreQuery, GetMoviesByGenreQueryResponse>
    {
        private readonly IGenericRepository<Movie> _movieRepository;
        public GetMoviesByGenreQueryHandler(IGenericRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;
        }
        public async Task<GetMoviesByGenreQueryResponse> Handle(GetMoviesByGenreQuery request, CancellationToken cancellationToken)
        {
            var movies =await _movieRepository.GetAllAsync();
            var result = movies.Where(x => x.Genres.Select(x=>x.ToLower()).Contains(request.genre.ToLower())).ToList();
            return new GetMoviesByGenreQueryResponse(result);
        }
    }
    public record GetMoviesByGenreQueryResponse(List<Movie> movies);
}
