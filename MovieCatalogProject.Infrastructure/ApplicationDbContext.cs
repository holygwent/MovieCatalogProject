using Microsoft.EntityFrameworkCore;
using MovieCatalogProject.Domain.Common;
using MovieCatalogProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCatalogProject.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase(databaseName: "MovieCatalogProjectDatabase");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<Movie>()
                .Property(e => e.Genres)
                .HasConversion(
                   v => string.Join(',', v),
                   v => v.Split(',',StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
