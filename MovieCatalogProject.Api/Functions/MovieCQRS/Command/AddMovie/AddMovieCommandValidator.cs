using FluentValidation;
using FluentValidation.AspNetCore;

namespace MovieCatalogProject.Api.Functions.MovieCQRS.Command.AddMovie
{
    public class AddMovieCommandValidator:AbstractValidator<AddMovieCommand>
    {
        public AddMovieCommandValidator()
        {
            RuleFor(t => t.name).NotEmpty();
            RuleFor(t => t.genres).NotEmpty();
            RuleFor(t => t.name).MinimumLength(2);
            RuleFor(t => t.name).Matches("[A-Z][.]*").WithMessage("Name must start from uppercase letter");
        }
    }
}
