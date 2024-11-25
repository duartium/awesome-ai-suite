namespace CharacterUniverse.Application.Common;

public static class Utils
{
	public static bool IsLocalEnvironment()
	{
		return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
			.Equals("Local", StringComparison.Ordinal);
	}
}
