using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using MovieCatalogProject.Domain.Common;
using MovieCatalogProject.Domain.Entities;
using MovieCatalogProject.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCatalogProject.Infrastructure.Test
{
    public class GenericRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<Movie> _movieRepository;
        public GenericRepositoryTests()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: "TestDbInMemory");

            var dbContextOptions = builder.Options;
            _context = new ApplicationDbContext();
            // Delete existing db before creating a new one
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            var logger = new Mock<ILogger<GenericRepository<Movie>>>().Object;
            _movieRepository = new GenericRepository<Movie>(_context, logger);

        }
        [Fact]
        public async Task AddAsync_WhenCorrectData_DbShouldHaveAddedElement()
        {
            //arrange
            string[] tab = { "horror", "comedy" };
            var movie = new Movie { Genres = tab, ReleaseDate = new DateTime(1997, 10, 10), Name = "Scary movie" };
            //act
            await _movieRepository.AddAsync(movie);
            //assert
            _context.Movies.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task AddAsync_WhenEntityWithSameIdAdded_DbShouldThrowError()
        {
            //arrange
            string[] tab = { "horror", "comedy" };
            var movie1 = new Movie { Genres = tab, ReleaseDate = new DateTime(1997, 10, 10), Name = "Scary movie", Id = Guid.Parse("85ec3bc8-756a-42e8-938e-ae59ea8b4caf") };
            var movie2 = new Movie { Genres = tab, ReleaseDate = new DateTime(1997, 10, 10), Name = "Scary movie", Id = Guid.Parse("85ec3bc8-756a-42e8-938e-ae59ea8b4caf") };
            //act and assert
            await _movieRepository.AddAsync(movie1);
            Assert.Throws<AggregateException>(() => _movieRepository.AddAsync(movie2).Wait());
        }
        [Fact]
        public async Task GetByIdAsync_WhenExecuted_DbShouldReturnMovieById()
        {
            //arrange
            string[] tab = { "horror", "comedy" };
            var movie1 = new Movie { Genres = tab, ReleaseDate = new DateTime(1997, 10, 10), Name = "Scary movie", Id = Guid.Parse("85ec3bc8-766a-42e8-938e-ae59ea8b4caf") };
            var movie2 = new Movie { Genres = tab, ReleaseDate = new DateTime(1997, 10, 10), Name = "Scary movie", Id = Guid.Parse("1145859e-1f16-41ad-b5ad-d18f649a0a08") };
            //act 
            await _movieRepository.AddAsync(movie1);
            await _movieRepository.AddAsync(movie2);
            var movie = await _movieRepository.GetByIdAsync(Guid.Parse("85ec3bc8-766a-42e8-938e-ae59ea8b4caf"));
            //assert
            movie.Should().NotBeNull();
            movie.Should().Be(movie1);

        }
        [Fact]
        public async Task GetByIdAsync_WhenGivenFalseId_DbShouldReturnNull()
        {
            //arrange
            //act 
            var movie = await _movieRepository.GetByIdAsync(Guid.Parse("85ec3bc8-756a-43e8-938e-ae59ea8b4caf"));
            //assert
            movie.Should().BeNull();
        }
        [Fact]
        public async Task GetAllAsync_WhenExecuted_DbShouldReturnAllElements()
        {
            //arrange
            string[] tab = { "horror", "comedy" };
            var movie1 = new Movie { Genres = tab, ReleaseDate = new DateTime(1997, 10, 10), Name = "Scary movie", Id = Guid.Parse("85ac3bc8-766a-42e8-938e-ae59ea8b4caf") };
            //act 
            await _movieRepository.AddAsync(movie1);
            var movies = await _movieRepository.GetAllAsync();
            //assert
            movies.Should().BeOfType<List<Movie>>();
            movies.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task Delete_WhenExecuted_DbShouldDeleteElement()
        {
            //arrange
            string[] tab = { "horror", "comedy" };
            var movie1 = new Movie { Genres = tab, ReleaseDate = new DateTime(1997, 10, 10), Name = "Scary movie",Id = Guid.Parse("701d2119-8d8d-4109-a839-f3583d3952e5") };
            //act 
            await _movieRepository.AddAsync(movie1);
            _movieRepository.Delete(movie1.Id);
            //assert
            _context.Movies.Should().NotContain(movie1);
        }






    }
}
