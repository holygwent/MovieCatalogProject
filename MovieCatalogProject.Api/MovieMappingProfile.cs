using AutoMapper;
using MovieCatalogProject.Api.Functions.MovieCQRS.Command;
using MovieCatalogProject.Domain.Entities;

namespace MovieCatalogProject.Api
{
    public class MovieMappingProfile:Profile
    {
        public MovieMappingProfile()
        {
            CreateMap<AddMovieCommand,Movie>()
                .ForMember(m=>m.Id,c=>c.AllowNull());
        }
    }
}
