using Shared.API;
using OrderService.Application;
using OrderService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.AddSharedDefaults(typeof(OrderService.Application.DependencyInjection).Assembly);
builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseSharedDefaults();

app.Run();