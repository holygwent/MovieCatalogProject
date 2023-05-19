using AutoMapper;
using MediatR;
using MovieCatalogProject.Domain.Common;
using MovieCatalogProject.Domain.Entities;
using MovieCatalogProject.Infrastructure.Exceptions;

namespace MovieCatalogProject.Api.Functions.MovieCQRS.Query
{
    public record GetLastMovieQuery() : IRequest<GetLastMovieQueryResponse>;

    public class GetLastMovieQueryHandler : IRequestHandler<GetLastMovieQuery, GetLastMovieQueryResponse>
    {
        private readonly IGenericRepository<Movie> _movieRepository;
        private readonly IMapper _mapper;
        public GetLastMovieQueryHandler(IGenericRepository<Movie> movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }
        public async Task<GetLastMovieQueryResponse> Handle(GetLastMovieQuery request, CancellationToken cancellationToken)
        {
            var lastMovie = _movieRepository.GetAllAsync().Result.LastOrDefault();
            if (lastMovie is null)
                throw new NotFoundException("There is no movies in catalog");
            var response = _mapper.Map<GetLastMovieQueryResponse>(lastMovie);
            return response;
        }
    }
    public record GetLastMovieQueryResponse(Guid Id, string Name, DateTime ReleaseDate, string[] Genres);
}
