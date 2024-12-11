using CharacterUniverse.Infraestructure.AI;
using CharacterUniverse.Infraestructure.Characters.Helpers;
using Spectre.Console;
using System.Text;

namespace CharacterUniverse.Application;

public class Application
{
	private readonly AISemanticService _semanticService;
	public Application(AISemanticService aISemanticService)
    {
		_semanticService = aISemanticService;
	}
    public void Run()
	{
		Console.OutputEncoding = Encoding.UTF8;
		
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
				.AddChoices(CharacterJsonHelper.GetAllCharacterNames())
			);

		var character = CharacterJsonHelper.GetCharacterByName(selectedCharacterName);

		AnsiConsole.MarkupLine($"[blue]¡Great! You has selected:[/] [white on green]{selectedCharacterName}[/]");

		var initialPrompt = character.Description;
		_semanticService.InvokePromptAsync(initialPrompt);

		while (true)
		{
			var userInput = Console.ReadLine();

			if(string.IsNullOrWhiteSpace(userInput))
				continue;

			if (!character.Plugin.IsCustomPlugin)
			{
				_semanticService.InvokePromptAsync(userInput.ToString());
			}else 
			{
				var result = Task.Run(async () => await _semanticService
					.InvokeCustomPluginAsync(character.Plugin.Name, userInput)
				);
				AnsiConsole.MarkupLine($"[blue on white]{result.Result}[/]");
			}
				
		}
	}
}

