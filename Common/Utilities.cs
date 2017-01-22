public static class Utilities
{
	/// <summary>
	/// Opens the web.
	/// </summary>
	/// <param name="url">URL.</param>
	public static void OpenWeb(string url)
	{
		try
		{
			System.Diagnostics.Process.Start(url);
		}
		catch { }
	}
}