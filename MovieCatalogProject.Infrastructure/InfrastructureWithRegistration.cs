using Microsoft.Extensions.DependencyInjection;
using MovieCatalogProject.Domain.Common;
using MovieCatalogProject.Domain.Entities;
using MovieCatalogProject.Infrastructure.Middleware;
using MovieCatalogProject.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCatalogProject.Infrastructure
{
    public static class InfrastructureWithRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped(typeof(IGenericRepository<Movie>), typeof(GenericRepository<Movie>));
            services.AddScoped<ErrorHandlingMiddleware>();
        }
        
    }
}
