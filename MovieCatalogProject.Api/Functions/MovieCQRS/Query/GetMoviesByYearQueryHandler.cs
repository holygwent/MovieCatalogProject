using MediatR;
using MovieCatalogProject.Domain.Common;
using MovieCatalogProject.Domain.Entities;

namespace MovieCatalogProject.Api.Functions.MovieCQRS.Query
{
    public record GetMoviesByYearQuery(int year):IRequest<GetMoviesByYearQueryResponse>;
    public class GetMoviesByYearQueryHandler : IRequestHandler<GetMoviesByYearQuery, GetMoviesByYearQueryResponse>
    {
        private readonly IGenericRepository<Movie> _movieRepository;
        public GetMoviesByYearQueryHandler(IGenericRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;
        }
        public async Task<GetMoviesByYearQueryResponse> Handle(GetMoviesByYearQuery request, CancellationToken cancellationToken)
        {
            var movies = _movieRepository.GetAllAsync().Result.Where(x => x.ReleaseDate.Year == request.year).ToList();
            return new GetMoviesByYearQueryResponse(movies);
        }
    }
    public record GetMoviesByYearQueryResponse(List<Movie> Movies);
}
