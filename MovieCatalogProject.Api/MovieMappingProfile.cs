using AutoMapper;
using MovieCatalogProject.Api.Functions.MovieCQRS.Command;
using MovieCatalogProject.Api.Functions.MovieCQRS.Query;
using MovieCatalogProject.Domain.Entities;

namespace MovieCatalogProject.Api
{
    public class MovieMappingProfile:Profile
    {
        public MovieMappingProfile()
        {
            CreateMap<AddMovieCommand,Movie>()
                .ForMember(m=>m.Id,c=>c.AllowNull());
            CreateMap<Movie, GetLastMovieQueryResponse>();
        }
    }
}
