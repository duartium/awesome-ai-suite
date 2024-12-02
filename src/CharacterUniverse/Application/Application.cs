using CharacterUniverse.Application.Common;
using CharacterUniverse.Infraestructure.AI;
using CharacterUniverse.Infraestructure.Characters.Helpers;
using Microsoft.SemanticKernel;
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
			
			_semanticService.InvokePromptAsync(userInput.ToString());
		}
	}

	public async void RunCustomPlugin()
	{
		string filePath = Path.Combine(
			Utils.GetProjectRootDirectory(),
			"Infraestructure/Prompts");

		var kernel = Kernel.CreateBuilder()
			.Build();

		
		var plugins = kernel.CreatePluginFromPromptDirectory(filePath);
		string input = "G, C";

		var result = await kernel.InvokeAsync(
			plugins["SuggestChords"],
			new() { { "startingChords", input } }
		);

		AnsiConsole.MarkupLine($"[blue on white]{result}[/]");
	}
}

