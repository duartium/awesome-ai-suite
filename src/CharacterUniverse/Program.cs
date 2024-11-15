using CharacterUniverse.Application;
using CharacterUniverse.Application.Middleware;

var  app = new Application();

var pipeline = new MiddlewarePipeline()
	.Use(new CharacterUniverseHandlingMiddleware())
	.SetFinalAction(app.Run);

pipeline.Execute();