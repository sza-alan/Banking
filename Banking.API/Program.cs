using Banking.Api.Middleware;
using Banking.Api.Projections;
using Banking.API.Application.Behaviors;
using Banking.API.Application.Services;
using Banking.API.Data;
using Banking.API.EventStore;
using Banking.API.IntegrationEvents;
using Banking.API.Models.Snapshots;
using Banking.API.Projections;
using Banking.API.Repositories;
using Banking.API.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);

    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));

    cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));

    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<AccountProjection>();
builder.Services.AddScoped<ProjectionReplayService>();
builder.Services.AddScoped<ISnapshotStore, SnapshotStore>();
builder.Services.AddScoped<AccountAggregateLoader>();
builder.Services.AddScoped<IIntegrationEventPublisher, ConsoleIntegrationEventPublisher>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
