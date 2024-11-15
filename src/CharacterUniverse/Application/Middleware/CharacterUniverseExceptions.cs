using Spectre.Console;

namespace CharacterUniverse.Application.Middleware;
public interface IMiddleware
{
	void Execute(Action next);
}
public class CharacterUniverseHandlingMiddleware : IMiddleware
{
	public void Execute(Action next)
	{
		try { next(); } catch (Exception ex) { AnsiConsole.WriteException(ex); }
	}
}
