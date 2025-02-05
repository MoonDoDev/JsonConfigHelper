using FluentResults;
using Newtonsoft.Json;

namespace JsonConfigHelper;

/// <summary>
/// Clase que define las extensiones necesarios para convertir los datos
/// y así interactuar con los archivos de configuración en formato JSON.
/// </summary>
public static class JsonExtensions
{
    /// <summary>
    /// Método con el que convertimos una cadena en formato JSON a un objecto <T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonData"></param>
    /// <returns>Retornamos un objeto de tipo <T> correspondiente al JSON suministrado.</returns>
    public static T JsonToObject<T>( this string jsonData )
    {
        JsonSerializerSettings settings = new()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto
        };

        return JsonConvert.DeserializeObject<T>( jsonData, settings )!;
    }

    /// <summary>
    /// Método con el que creamos un archivo de configuración partiendo de un objecto JSON <T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender"></param>
    /// <param name="fileName"></param>
    /// <param name="format"></param>
    /// <returns>Retornamos un <see cref="FluentResult"/> con el resultado de la operación.</returns>
    public static Result<bool> JsonToFile<T>( this T sender, string fileName, bool format = true )
    {
        try
        {
            JsonSerializerSettings settings = new()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto
            };

            File.WriteAllText( fileName, JsonConvert.SerializeObject( sender, format ? settings : null ) );
            return Result.Ok();
        }
        catch( Exception ex )
        {
            return Result.Fail( ex.Message );
        }
    }
}
