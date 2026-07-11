using ProductService.Application;
using ProductService.Infrastructure;
using Shared.API;
using Shared.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Logging
builder.Host.UseSharedSerilog("ProductService");

// Register application services first (which includes MediatR via AddSharedApplication)
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Then add shared defaults (which depends on MediatR being registered)
builder.AddSharedDefaults(typeof(ProductService.Application.DependencyInjection).Assembly);

var app = builder.Build();

app.UseSharedSerilogRequestLogging();

app.UseSharedDefaults();

app.MapControllers();

app.Run();