using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCatalogProject.Api.Controllers;
using MovieCatalogProject.Api.Functions.MovieCQRS.Command.AddMovie;
using MovieCatalogProject.Api.Functions.MovieCQRS.Query.GetLastMovie;
using MovieCatalogProject.Api.Functions.MovieCQRS.Query.GetMoviesByGenre;
using MovieCatalogProject.Api.Functions.MovieCQRS.Query.GetMoviesByYear;
using MovieCatalogProject.Domain.Entities;


namespace MovieCatalogProject.Api.Test
{
    public class MovieControllerTests
    {
        private readonly Mock<IMediator> _fakeMediator;
        private readonly Mock<IValidator<AddMovieCommand>> _fakeValidator;

        public MovieControllerTests()
        {
            _fakeMediator = new Mock<IMediator>();
            _fakeValidator = new Mock<IValidator<AddMovieCommand>>();

        }

        [Fact]
        public async Task AddMovie_WhenCorrectData_ShouldReturnStatusCode201()
        {
            //arrange
            string[] tab = { "horror", "comedy" };
            AddMovieCommand dto = new AddMovieCommand("Scary movie", new DateTime(2017, 10, 19), tab);
            _fakeMediator.Setup(m => m.Send(dto, It.IsAny<CancellationToken>()));
            var movieController = new MovieController(_fakeMediator.Object, _fakeValidator.Object);
            _fakeValidator.Setup(x => x.ValidateAsync(dto, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new ValidationResult());
            //act
            var actionResult = (StatusCodeResult)await movieController.AddMovie(dto);
            //assert
            actionResult.StatusCode.Should().Be(201);

        }
        [Fact]
        public async Task AddMovie_WhenIncorrectData_ShouldReturnStatusCode400()
        {
            //arrange
            string[] tab = { "horror", "comedy" };
            var validationFailure = new ValidationFailure("Name", "something went wrong");
            IEnumerable<ValidationFailure> validationFailures = new[] { validationFailure };
            AddMovieCommand dto = new AddMovieCommand("Scary movie", new DateTime(2017, 10, 19), tab);
            _fakeMediator.Setup(m => m.Send(dto, It.IsAny<CancellationToken>()));
            var movieController = new MovieController(_fakeMediator.Object, _fakeValidator.Object);
            _fakeValidator.Setup(x => x.ValidateAsync(dto, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new ValidationResult(validationFailures));
            //act
            var actionResult = (ObjectResult)await movieController.AddMovie(dto);
            //assert
            actionResult.StatusCode.Should().Be(400);

        }
        [Fact]
        public async Task AddMovie_WhenCorrectData_ShouldExecuteMediatorSendFunctionOnce()
        {
            //arrange
            string[] tab = { "horror", "comedy" };
            AddMovieCommand dto = new AddMovieCommand("Scary movie", new DateTime(2017, 10, 19), tab);
            _fakeMediator.Setup(m => m.Send(dto, It.IsAny<CancellationToken>()));
            var movieController = new MovieController(_fakeMediator.Object, _fakeValidator.Object);
            _fakeValidator.Setup(x => x.ValidateAsync(dto, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(new ValidationResult());
            //act
            var actionResult = await movieController.AddMovie(dto);
            //assert
            _fakeMediator.Verify(x => x.Send(dto, It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task GetLastMovie_WhenMovieFound_ShouldReturnStatusCode200AndValueOfTypeGetLastMovieQueryResponse()
        {
            //arrange
            string[] tab = { "horror", "comedy" };
            var getLastMovieQueryResponse = new GetLastMovieQueryResponse(Guid.NewGuid(), "Scary movie", DateTime.Now, tab);
            _fakeMediator.Setup(m => m.Send(new GetLastMovieQuery(), It.IsAny<CancellationToken>())).ReturnsAsync(getLastMovieQueryResponse);
            var movieController = new MovieController(_fakeMediator.Object, _fakeValidator.Object);
            //act
            var actionResult = await movieController.GetLastMovie();
            //assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType(typeof(OkObjectResult));
            var okActionResult = (ObjectResult)actionResult;
            okActionResult.StatusCode.Should().Be(200);
            okActionResult.Value.Should().NotBeNull();
            okActionResult.Value.Should().BeOfType(typeof(GetLastMovieQueryResponse));
            Assert.Equal(getLastMovieQueryResponse, okActionResult.Value);
        }

        [Fact]
        public async Task GetLastMovie_WhenMovieFound_ShouldExecuteMediatorSendFunctionOnce()
        {
            //arrange
            string[] tab = { "horror", "comedy" };
            var getLastMovieQueryResponse = new GetLastMovieQueryResponse(Guid.NewGuid(), "Scary movie", DateTime.Now, tab);
            _fakeMediator.Setup(m => m.Send(new GetLastMovieQuery(), It.IsAny<CancellationToken>())).ReturnsAsync(getLastMovieQueryResponse);
            var movieController = new MovieController(_fakeMediator.Object, _fakeValidator.Object);
            //act
            var actionResult = await movieController.GetLastMovie();
            //assert
            _fakeMediator.Verify(x => x.Send(new GetLastMovieQuery(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetMoviesByGenre_WhenMovieFound_ShouldExecuteMediatorSendFunctionOnce()
        {
            //arrange
            string genre = "horror";
            string[] tabMovie1 = { "horror", "comedy" };
            string[] tabMovie2 = { "horror", "action" };
            List<Movie> movies = new List<Movie>() {
                new Movie{Id = Guid.NewGuid(),Name="Scary movie",ReleaseDate=DateTime.Now,Genres=tabMovie1 },
                new Movie{Id = Guid.NewGuid(),Name="Horror movie",ReleaseDate=DateTime.Now,Genres=tabMovie2}
            };
            _fakeMediator.Setup(m => m.Send(new GetMoviesByGenreQuery(genre), It.IsAny<CancellationToken>())).ReturnsAsync(new GetMoviesByGenreQueryResponse(movies));
            var movieController = new MovieController(_fakeMediator.Object, _fakeValidator.Object);
            //act
            var actionResult = await movieController.GetMoviesByGenre(genre);
            //assert
            _fakeMediator.Verify(x => x.Send(new GetMoviesByGenreQuery(genre), It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task GetMoviesByGenre_WhenMovieFound_ShouldReturnStatusCode200AndValueOfTypeGetMoviesByGenreQueryResponse()
        {
            //arrange
            string genre = "horror";
            string[] tabMovie1 = { "horror", "comedy" };
            string[] tabMovie2 = { "horror", "action" };
            List<Movie> movies = new List<Movie>() {
                new Movie{Id = Guid.NewGuid(),Name="Scary movie",ReleaseDate=DateTime.Now,Genres=tabMovie1 },
                new Movie{Id = Guid.NewGuid(),Name="Horror movie",ReleaseDate=DateTime.Now,Genres=tabMovie2}
            };
            _fakeMediator.Setup(m => m.Send(new GetMoviesByGenreQuery(genre), It.IsAny<CancellationToken>())).ReturnsAsync(new GetMoviesByGenreQueryResponse(movies));
            var movieController = new MovieController(_fakeMediator.Object, _fakeValidator.Object);
            //act
            var actionResult = await movieController.GetMoviesByGenre(genre);
            //assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType(typeof(OkObjectResult));
            var okActionResult = (ObjectResult)actionResult;
            okActionResult.StatusCode.Should().Be(200);
            okActionResult.Value.Should().NotBeNull();
            okActionResult.Value.Should().BeOfType(typeof(GetMoviesByGenreQueryResponse));
            Assert.Equal(new GetMoviesByGenreQueryResponse(movies), okActionResult.Value);
        }

        [Fact]
        public async Task GetMoviesByYear_WhenMoviesFound_ShouldExecuteMediatorSendFunctionOnce()
        {
            //arrange
            int year = 2023;
            string[] tabMovie1 = { "horror", "comedy" };
            string[] tabMovie2 = { "horror", "action" };
            List<Movie> movies = new List<Movie>() {
                new Movie{Id = Guid.NewGuid(),Name="Scary movie",ReleaseDate=DateTime.Now,Genres=tabMovie1 },
                new Movie{Id = Guid.NewGuid(),Name="Horror movie",ReleaseDate=DateTime.Now,Genres=tabMovie2}
            };
            _fakeMediator.Setup(m => m.Send(new GetMoviesByYearQuery(year), It.IsAny<CancellationToken>())).ReturnsAsync(new GetMoviesByYearQueryResponse(movies));
            var movieController = new MovieController(_fakeMediator.Object, _fakeValidator.Object);
            //act
            var actionResult = await movieController.GetMoviesByYear(year);
            //assert
            _fakeMediator.Verify(x => x.Send(new GetMoviesByYearQuery(year), It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task GetMoviesByYear_WhenMoviesFound_ShouldReturnStatusCode200AndValueOfTypeGetMoviesByYearQueryResponseObject()
        {
            //arrange
            int year = 2023;
            string[] tabMovie1 = { "horror", "comedy" };
            string[] tabMovie2 = { "horror", "action" };
            List<Movie> movies = new List<Movie>() {
                new Movie{Id = Guid.NewGuid(),Name="Scary movie",ReleaseDate=DateTime.Now,Genres=tabMovie1 },
                new Movie{Id = Guid.NewGuid(),Name="Horror movie",ReleaseDate=DateTime.Now,Genres=tabMovie2}
            };
            _fakeMediator.Setup(m => m.Send(new GetMoviesByYearQuery(year), It.IsAny<CancellationToken>())).ReturnsAsync(new GetMoviesByYearQueryResponse(movies));
            var movieController = new MovieController(_fakeMediator.Object, _fakeValidator.Object);
            //act
            var actionResult = await movieController.GetMoviesByYear(year);
            //assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType(typeof(OkObjectResult));
            var okActionResult = (ObjectResult)actionResult;
            okActionResult.StatusCode.Should().Be(200);
            okActionResult.Value.Should().NotBeNull();
            okActionResult.Value.Should().BeOfType(typeof(GetMoviesByYearQueryResponse));
            Assert.Equal(new GetMoviesByYearQueryResponse(movies), okActionResult.Value);
        }


    }
}
