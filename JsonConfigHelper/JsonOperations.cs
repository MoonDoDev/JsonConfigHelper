using FluentResults;

namespace JsonConfigHelper;

/// <summary>
/// Clase que nos permitirá leer/escribir las secciones de/hacia un archivo de configuración en formato JSON.
/// </summary>
/// <typeparam name="T">Clase base que definirá el objeto con el que se leeran y escribirán los datos.</typeparam>
/// <param name="settingsFileNamePath">Ruta y nombre del archivo de configuración en formato JSON.</param>
public class JsonOperations<T>( string settingsFileNamePath ) where T : class
{
    /// <summary>
    /// Ruta y nombre del archivo de configuración en formato JSON con el que se trabajará
    /// </summary>
    private readonly string _settingsFileNamePath = settingsFileNamePath;

    /// <summary>
    /// Definición del método delegado que se debe implementar para validar/actualizar los datos del archivo de configuración en formato JSON.
    /// </summary>
    /// <param name="jsonCurrentData">Objeto de tipo <T> definido, y en donde se entregarán los datos leídos en el archivo JSON.</param>
    /// <returns>Debemos retornar un <see cref="FluentResults"/> con el objeto de tipo <T> definido, con los datos actualizados.</returns>
    public delegate Result<T> UpdateParamValues( T jsonCurrentData );

    /// <summary>
    /// Leemos la información del archivo de configuración suministrado en formato JSON, y la retornamos como un objeto de tipo <T>.
    /// </summary>
    /// <returns>Retornamos un <see cref="FluentResult"/> con un objeto del tipo <T> definido, con los datos leídos en el archivo JSON.</returns>
    public Result<T> LoadConfigFileData()
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty( _settingsFileNamePath, nameof( _settingsFileNamePath ) );

            var jsonData = File.ReadAllText( _settingsFileNamePath );
            var jsonConfigData = jsonData.JsonToObject<T>();

            ArgumentNullException.ThrowIfNull( jsonConfigData, nameof( jsonConfigData ) );

            return Result.Ok( jsonConfigData );
        }
        catch( Exception ex )
        {
            return Result.Fail( $"LoadFileData( Throws an exception => {ex.Message} )" );
        }
    }

    /// <summary>
    /// Guardamos en el archivo de configuración definido en formato JSON, los datos retornados por el método delegado "updateMethod".
    /// </summary>
    /// <param name="callbackUpdateMethod">Método delegado de tipo <see cref="UpdateParamValues"/> que se invocará para
    /// permitir la validación/actualización de los datos leídos en el archivo de configuración.</param>
    /// <returns>Retornamos un <see cref="FluentResult"/> con el resultado de la operación.</returns>
    public Result<bool> SaveDataInConfigFile( UpdateParamValues callbackUpdateMethod )
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty( _settingsFileNamePath, nameof( _settingsFileNamePath ) );

            var jsonData = File.ReadAllText( _settingsFileNamePath );
            var jsonCurrentData = jsonData.JsonToObject<T>();

            var updResult = callbackUpdateMethod( jsonCurrentData );

            if( updResult.IsSuccess )
            {
                var jsonNewData = updResult.Value;
                ArgumentNullException.ThrowIfNull( jsonNewData, nameof( jsonNewData ) );

                return jsonNewData.JsonToFile( _settingsFileNamePath );
            }

            return Result.Fail( $"SaveDataToConfigFile( {updResult.Reasons[ 0 ]} )" );
        }
        catch( Exception ex )
        {
            return Result.Fail( $"SaveDataToConfigFile( Throws an exception => {ex.Message} )" );
        }
    }
}
