
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MoviesTicket.API.Middleware;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddMovies(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddAuthorization();
builder.Services.AddAuthenticationWithBearer(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
