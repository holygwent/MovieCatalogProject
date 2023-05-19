using MediatR;
using MovieCatalogProject.Domain.Common;
using MovieCatalogProject.Domain.Entities;

namespace MovieCatalogProject.Api.Functions.MovieCQRS.Query.GetMoviesByYear
{
    public record GetMoviesByYearQuery(int year) : IRequest<GetMoviesByYearQueryResponse>;
    public class GetMoviesByYearQueryHandler : IRequestHandler<GetMoviesByYearQuery, GetMoviesByYearQueryResponse>
    {
        private readonly IGenericRepository<Movie> _movieRepository;
        public GetMoviesByYearQueryHandler(IGenericRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;
        }
        public async Task<GetMoviesByYearQueryResponse> Handle(GetMoviesByYearQuery request, CancellationToken cancellationToken)
        {
            var movies = await _movieRepository.GetAllAsync();
            var result = movies.Where(x => x.ReleaseDate.Year == request.year).ToList();
            return new GetMoviesByYearQueryResponse(result);
        }
    }
    public record GetMoviesByYearQueryResponse(List<Movie> Movies);
}
