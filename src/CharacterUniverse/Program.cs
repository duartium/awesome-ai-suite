using CharacterUniverse.Application;
using CharacterUniverse.Application.Middleware;
using CharacterUniverse.Infraestructure.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

#region settings application
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

var builder = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
	.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
	.AddEnvironmentVariables()
	.AddCommandLine(args);

var configuration = builder.Build();
var serviceProvider = new ServiceCollection()
	.AddSingleton<IConfiguration>(configuration)
	.AddTransient<Application>()
	.AddTransient<AISemanticService>()
	.BuildServiceProvider();

#endregion

var app = serviceProvider.GetRequiredService<Application>();

var pipeline = new MiddlewarePipeline()
	.Use(new CharacterUniverseHandlingMiddleware())
	.SetFinalAction(app.Run);

pipeline.Execute();