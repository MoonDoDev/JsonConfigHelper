namespace JsonConfigHelper;

/// <summary>
/// Clase que define del atributo para las propiedades, con la que se podrá indicar que ésta contiene información cifrada - 
/// (La implementación del cifrado en las propieades queda a cargo de quien defina y construya la clase o record).
/// </summary>
/// <param name="value">Parámetro de tipo bool para indicar TRUE si el contenido de la propiedad esta cifrado o FALSE en caso contrario.</param>
[System.AttributeUsage( System.AttributeTargets.Property )]
public class PropertyIsCoded( bool value ): System.Attribute
{
    /// <summary>
    /// 
    /// </summary>
    private readonly bool _value = value;

    /// <summary>
    /// 
    /// </summary>
    public bool Value => _value;
}

/// <summary>
/// Clase estática con la que se podrá validar si una propiedad tiene el atribute "PropertyIsEncrypted"
/// </summary>
public static class JsonProperty
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static bool IsCoded<T>( string propertyName )
    {
        return ( Attribute.GetCustomAttribute( typeof( T ).GetProperty( propertyName )!,
            typeof( PropertyIsCoded ) ) as PropertyIsCoded )!.Value;
    }
}
