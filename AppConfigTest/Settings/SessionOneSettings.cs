namespace AppConfigTest.Settings;

/// <summary>
/// 
/// </summary>
public record SessionOneSettings
{
	/// <summary>
	/// 
	/// </summary>
	public bool CreateLogFile { get; set; }
	
	/// <summary>
	/// 
	/// </summary>
	public string? LogFileLocation { get; set; }
}
