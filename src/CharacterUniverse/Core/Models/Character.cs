namespace CharacterUniverse.Core.Models;

public class Character
{
    public string Code { get; set; } = "";
	public string Name { get; set; } = "";
	public string Description { get; set; } = "";
    public List<string> Objectives { get; set; } = new List<string>();
}
