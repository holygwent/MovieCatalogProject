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
//validation
builder.Services.AddScoped<IValidator<AddMovieCommand>, AddMovieCommandValidator>();
builder.Host.UseNLog();
var app = builder.Build();
// Configure the HTTP request pipeline.
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
