namespace CharacterUniverse.Application.Common;

public static class Utils
{
	public static bool IsLocalEnvironment()
	{
		return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
			.Equals("Local", StringComparison.Ordinal);
	}

	public static string GetProjectRootDirectory()
	{
		var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
		while (Directory.GetParent(currentDirectory).Name != "bin")
		{
			currentDirectory = Directory.GetParent(currentDirectory).FullName;
		}
		return Directory.GetParent(currentDirectory).Parent.FullName;
	}
}
