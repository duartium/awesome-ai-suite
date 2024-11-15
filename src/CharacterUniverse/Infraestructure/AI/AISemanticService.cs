using Microsoft.SemanticKernel;
using Spectre.Console;

namespace CharacterUniverse.Infraestructure.AI;

public class AISemanticService
{
    private readonly Kernel _kernel;
    public AISemanticService()
    {
        var builder = Kernel.CreateBuilder();
        builder.AddOpenAIChatCompletion(
            modelId: "gpt-4",
            apiKey: ""
			);
        _kernel = builder.Build();
    }

	public void InvokePromptAsync(string prompt)
    {
        try
        {
			
            var response = Task.Run(async () => await _kernel.InvokePromptAsync(prompt))
                .Result;
            
			AnsiConsole.WriteLine(response.GetValue<string>());
			Console.ReadKey();
		}
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
