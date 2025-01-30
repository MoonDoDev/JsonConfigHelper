namespace AppConfigTest.Settings;

/// <summary>
/// 
/// </summary>
public record SessionTwoSettings
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
	public List<CollectionSettings>? CollectionParams { get; set; }
}
