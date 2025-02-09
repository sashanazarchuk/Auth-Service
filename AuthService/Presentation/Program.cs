using Scalar.AspNetCore;
using Infrastructure.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add ConnectionString
builder.Services.ConfigureDbContext(builder.Configuration);

//Add Identity
builder.Services.ConfigureIdentity();

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
