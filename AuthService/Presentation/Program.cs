using Scalar.AspNetCore;
using Infrastructure.Extensions;
using Application.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add ConnectionString
builder.Services.ConfigureDbContext(builder.Configuration);

//Add Identity
builder.Services.ConfigureIdentity();

//Add Fluent Validation
builder.Services.AddValidatorsFromAssembly(typeof(RegisterViewModelValidator).Assembly);

builder.Services.AddInfrastructureServices();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.Title = "Scalar API";
        opt.Theme = ScalarTheme.BluePlanet;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
