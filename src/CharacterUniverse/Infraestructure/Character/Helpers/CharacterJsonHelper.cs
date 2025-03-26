using CharacterUniverse.Application.Common;
using CharacterUniverse.Core.Models;
using Spectre.Console;
using System.Text.Json;

namespace CharacterUniverse.Infraestructure.Characters.Helpers;

public class CharacterJsonHelper
{
	private static readonly string filePath = Path.Combine(
		Utils.GetProjectRootDirectory(), 
		"Application/Settings/characters.json");

	public static List<Character> GetCharactersFromSettings()
	{
		if (!File.Exists(filePath))
			AnsiConsole.WriteException(new Exception($"File characters named {filePath} not found."));

		var jsonCharacters = File.ReadAllText(filePath);
		var characters = JsonSerializer.Deserialize<List<Character>>(jsonCharacters);
		
		if(characters is null || characters.Count == 0)
			AnsiConsole.WriteException(new Exception($"File characters named {filePath} not found.")); ;

		return characters;
	}

	public static Character GetCharacterByName(string name)
	{
		string shortName = name.Split(':')[0];

        var characters = GetCharactersFromSettings();

		var character = characters.FirstOrDefault( x => x.Name == shortName);
        
		if (character is null)
            throw new Exception($"Character {shortName} not found.");

        return character;
	}

	public static string[] GetAllCharacterNames()
	{
		var characters = GetCharactersFromSettings();

		var characterNames = characters
			.Select(x => $"{x.Name}: {x.ShortDescription}")
			.ToArray();

		

		return characterNames;
	}

	
}
