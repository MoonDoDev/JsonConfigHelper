namespace AppConfigTest.Settings;

/// <summary>
/// 
/// </summary>
[Flags]
public enum ConfigFileSession
{
    None = 0,
    LoggingParams = 1,
    ServiceParams = 2,
    SessionOneParams = 4,
    SessionTwoParams = 8
}

/// <summary>
/// 
/// </summary>
public record JsonConfigFile
{
	/// <summary>
	/// 
	/// </summary>
	public LoggingSettings? Logging { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public string? AllowedHosts { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public ServiceSettings? ServiceParams { get; set; }
}

/// <summary>
/// 
/// </summary>
public record LoggingSettings
{
	/// <summary>
	/// 
	/// </summary>
	public LogLevelSettings? LogLevel { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public EventLogSettings? EventLog { get; set; }
}

/// <summary>
/// 
/// </summary>
public record LogLevelSettings
{
	/// <summary>
	/// 
	/// </summary>
	public string? Default { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public string? JsonConfigHelper { get; set; }
}

/// <summary>
/// 
/// </summary>
public record EventLogSettings
{
	/// <summary>
	/// 
	/// </summary>
	public string? SourceName { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public string? LogName { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public LogLevelSettings? LogLevel { get; set; }
}
