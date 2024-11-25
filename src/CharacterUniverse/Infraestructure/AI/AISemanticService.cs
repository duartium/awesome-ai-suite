﻿using CharacterUniverse.Application.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Spectre.Console;

namespace CharacterUniverse.Infraestructure.AI;

public class AISemanticService
{
    private readonly IConfiguration _configuration;
    private readonly Kernel _kernel;

    public AISemanticService(IConfiguration configuration)
    {
		_configuration = configuration;
		var apiKey = _configuration["OPENAI_API_KEY"]; 
        
        if (string.IsNullOrEmpty(apiKey)) 
            throw new Exception("Api Key is not configured.");

		var builder = Kernel.CreateBuilder();
        builder.AddOpenAIChatCompletion(
            modelId: "gpt-4",
            apiKey: apiKey,
            httpClient: Utils.IsLocalEnvironment() 
                ? GetHttpClientForDevelopmentEnvironment()
				: null
			);
        _kernel = builder.Build();
    }

	public void InvokePromptAsync(string prompt)
    {
        try
        {
            prompt = ReplaceVariablesInPrompt(prompt);
            prompt = RefinesResponse(prompt);

			var response = AnsiConsole.Status()
                .Spinner(Spinner.Known.Clock)
                .Start("Thinking...", ctx =>
                {
                    var response = Task.Run(async () => await _kernel.InvokePromptAsync(prompt))
                    .Result;
                    return response;
                });
            
			AnsiConsole.WriteLine(response.GetValue<string>());
            Console.ReadKey();
		}
        catch (Exception ex)
        {
			AnsiConsole.WriteLine(ex.ToString());
        }
    }

    private string ReplaceVariablesInPrompt(string prompt)
    {
        string newPrompt = string.Empty;

        var language = _configuration
                .GetSection("AGENTS")["language"]?
                .ToString();
		newPrompt = prompt.Replace("@language", language);

        return newPrompt;
	}

    private string RefinesResponse(string prompt)
    {
        return prompt
            + "Keep your answers brief, engaging, and impactful, " +
            "focusing on delivering essential information efficiently.";
	}

    private HttpClient GetHttpClientForDevelopmentEnvironment()
    {
		var handler = new HttpClientHandler();
		handler.CheckCertificateRevocationList = false;
		return new HttpClient(handler);
	}
}
