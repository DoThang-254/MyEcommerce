using Shared.API;
using Template.Application;
using Template.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
//builder.AddSharedDefaults(typeof(UserService.Application.DependencyInjection).Assembly);
builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseSharedDefaults();

app.Run();