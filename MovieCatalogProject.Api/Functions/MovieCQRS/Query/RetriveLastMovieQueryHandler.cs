using AutoMapper;
using MediatR;
using MovieCatalogProject.Domain.Common;
using MovieCatalogProject.Domain.Entities;
using MovieCatalogProject.Infrastructure.Exceptions;

namespace MovieCatalogProject.Api.Functions.MovieCQRS.Query
{
    public record RetriveLastMovieQuery() : IRequest<RetriveLastMovieQueryResponse>;
    
    public class RetriveLastMovieQueryHandler : IRequestHandler<RetriveLastMovieQuery, RetriveLastMovieQueryResponse>
    {
        private readonly IGenericRepository<Movie> _movieRepository;
        private readonly IMapper _mapper;
        public RetriveLastMovieQueryHandler(IGenericRepository<Movie> movieRepository,IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }
        public async Task<RetriveLastMovieQueryResponse> Handle(RetriveLastMovieQuery request, CancellationToken cancellationToken)
        {
            var lastMovie = _movieRepository.GetAllAsync().Result.LastOrDefault();
            if (lastMovie is null)
                throw new NotFoundException("There is no movies in catalog");
            var response = _mapper.Map<RetriveLastMovieQueryResponse>(lastMovie);
            return response;
        }
    }
    public record RetriveLastMovieQueryResponse(Guid Id,string Name,DateTime ReleaseDate, string[] Genres );
}
