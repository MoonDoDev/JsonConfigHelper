namespace AppConfigTest.Settings;

/// <summary>
/// 
/// </summary>
public record ServiceSettings
{
	/// <summary>
	/// 
	/// </summary>
	public bool CreateLogFile { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public string? LogFileLocation { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public SessionOneSettings? SessionOneParams { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public SessionTwoSettings? SessionTwoParams { get; set; }
}
