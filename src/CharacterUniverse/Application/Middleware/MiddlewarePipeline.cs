namespace CharacterUniverse.Application.Middleware;

public class MiddlewarePipeline
{
	private readonly List<IMiddleware> _middlewares = new List<IMiddleware>();
	private Action _finalAction;
	public MiddlewarePipeline Use(IMiddleware middleware)
	{
		_middlewares.Add(middleware);
		return this;
	}
	public MiddlewarePipeline SetFinalAction(Action finalAction)
	{
		_finalAction = finalAction;
		return this;
	}
	public void Execute()
	{
		Action next = _finalAction;
		for (int i = _middlewares.Count - 1; i >= 0; i--)
		{
			var middleware = _middlewares[i]; 
			var currentNext = next;
			next = () => middleware.Execute(currentNext);
		}
		next();
	}
}
