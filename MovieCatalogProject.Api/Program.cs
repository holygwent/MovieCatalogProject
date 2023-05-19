using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using MovieCatalogProject.Api.Functions.MovieCQRS.Command.AddMovie;
using MovieCatalogProject.Infrastructure;
using MovieCatalogProject.Infrastructure.Middleware;
using NLog.Web;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureServices();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseNLog();
//validation
builder.Services.AddScoped<IValidator<AddMovieCommand>, AddMovieCommandValidator>();

var app = builder.Build();
app.UseHttpLogging();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
