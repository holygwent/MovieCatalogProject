using FakeItEasy;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCatalogProject.Api.Controllers;
using MovieCatalogProject.Api.Functions.MovieCQRS.Command.AddMovie;
using MovieCatalogProject.Api.Functions.MovieCQRS.Query.GetLastMovie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MovieCatalogProject.Api.Test
{
    public class MovieControllerTests
    {
        private readonly Mock<IMediator> _fakeMediator;
        private readonly Mock<IValidator<AddMovieCommand>> _fakeValidator;

        public MovieControllerTests()
        {
            _fakeMediator = new Mock<IMediator>();
            _fakeValidator =new Mock<IValidator<AddMovieCommand>>();
        
       
        }
        [Fact]
        public async Task GetLastMovie_WhenCorrect_CheckingResponse()
        {
            //arrange
            string[] tab = { "horror","comedy" };
            var getLastMovieQueryResponse = new GetLastMovieQueryResponse(Guid.NewGuid(), "Scary movie", DateTime.Now, tab);
            _fakeMediator.Setup(m => m.Send(new GetLastMovieQuery(), It.IsAny<CancellationToken>())).ReturnsAsync(getLastMovieQueryResponse);
            var movieController = new MovieController(_fakeMediator.Object, _fakeValidator.Object);
            //act
            var actionResult = await movieController.GetLastMovie();
            //assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType(typeof(OkObjectResult));
            var okActionResult = (OkObjectResult)actionResult;
            okActionResult.StatusCode.Should().Be(200);
            okActionResult.Value.Should().NotBeNull();
            okActionResult.Value.Should().BeOfType(typeof(GetLastMovieQueryResponse));
            Assert.Equal(getLastMovieQueryResponse, okActionResult.Value);
        }
    }
}
