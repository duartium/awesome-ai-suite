using CharacterUniverse.Infraestructure.Characters.Helpers;
using Spectre.Console;

namespace CharacterUniverse.Application;

public class Application
{
	public void Run()
	{
		AnsiConsole.Write(
		new FigletText("Character Universe")
			.LeftJustified()
			.Color(Color.Blue));

		AnsiConsole.WriteLine("Select your character");
		var selectedCharacterName = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
			.Title("")
			.PageSize(10)
			.MoreChoicesText("[grey](Move up and down to reveal more characters)[/]")
			.AddChoices(new[]
			{
				"Rosie", "Doctor"
			}));

		var character = CharacterJsonHelper.GetCharacterByName(selectedCharacterName);

		AnsiConsole.Markup($"[blue]¡Great! You has selected:[/] [white on green]{selectedCharacterName}[/]");
	}
}

