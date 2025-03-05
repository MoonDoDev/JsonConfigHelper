namespace AppConfigTest.Settings;

using JsonConfigHelper;

/// <summary>
/// 
/// </summary>
public record CollectionSettings
{
    /// <summary>
    /// 
    /// </summary>
    [PropertyIsCoded( true )]
    public string? CollectionParamOne { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [PropertyIsCoded( false )]
    public string? CollectionParamTwo { get; set; }
}

