using Api.Contracts;
using Api.Extensions;
using Data;
using Domain.Users;
using Microsoft.AspNetCore.Mvc;
using UseCases.Users;
using UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenApiDocument()
    .AddOpenApi()
    .AddData()
    .AddUseCases();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenApi();
    app.UseSwaggerUi();
    builder.Services.AddEndpointsApiExplorer();
}

app.UseHttpsRedirection();

app.MapPost("/users", async ([FromBody] AddUserRequest request, UserUseCases useCases) =>
    await useCases.AddUser
        .ExecuteAsync(request)
        .ToHttpResult())
    .WithName("AddUser");

app.Run();