using AppConfigTest.Settings;
using FluentResults;
using JsonConfigHelper;

namespace AppConfigTest;

/// <summary>
/// 
/// </summary>
public class Program
{
    /// <summary>
    /// 
    /// </summary>
    private static ConfigFileSession s_sessionsToSave = ConfigFileSession.None;

    /// <summary>
    /// 
    /// </summary>
    private static JsonConfigFile s_appSettings = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentAppSettings"></param>
    /// <returns></returns>
    public static Result<JsonConfigFile> UpdateJsonAppSettings( JsonConfigFile currentAppSettings )
    {
        // Realizamos la validación de cada una de las secciones del archivo de configuración
        ArgumentNullException.ThrowIfNull( currentAppSettings, nameof( currentAppSettings ) );
        ArgumentNullException.ThrowIfNull( currentAppSettings.Logging, nameof( currentAppSettings.Logging ) );

        ArgumentNullException.ThrowIfNull( currentAppSettings.ServiceParams,
            nameof( currentAppSettings.ServiceParams ) );

        ArgumentNullException.ThrowIfNull( currentAppSettings.ServiceParams.SessionOneParams,
            nameof( currentAppSettings.ServiceParams.SessionOneParams ) );

        ArgumentNullException.ThrowIfNull( currentAppSettings.ServiceParams.SessionTwoParams,
            nameof( currentAppSettings.ServiceParams.SessionTwoParams ) );

        ArgumentNullException.ThrowIfNull( currentAppSettings.ServiceParams.SessionTwoParams.CollectionParams,
            nameof( currentAppSettings.ServiceParams.SessionTwoParams.CollectionParams ) );

        Console.Clear();
        Console.WriteLine( "Datos actuales antes de cambiar de configuración =>\r\n" );
        Console.WriteLine( $"Logging.LogLevel.JsonConfigHelper( {currentAppSettings.Logging.LogLevel!.JsonConfigHelper} )" );
        Console.WriteLine( $"Logging.EventLog.LogLevel.JsonConfigHelper( {currentAppSettings.Logging.EventLog!.LogLevel!.JsonConfigHelper} )" );
        Console.WriteLine( $"ServiceParams.LogFileLocation( {currentAppSettings.ServiceParams.LogFileLocation} )" );
        Console.WriteLine( $"ServiceParams.SessionOneParams.LogFileLocation( {currentAppSettings.ServiceParams.SessionOneParams.LogFileLocation} )" );
        Console.WriteLine( $"ServiceParams.SessionTwoParams.LogFileLocation( {currentAppSettings.ServiceParams.SessionTwoParams.LogFileLocation} )" );

        foreach( var value in currentAppSettings.ServiceParams.SessionTwoParams.CollectionParams )
        {
            Console.WriteLine( $"ServiceParams.SessionTwoParams.CollectionParams[].CollectionParamOne( {value.CollectionParamOne} )" );
            Console.WriteLine( $"ServiceParams.SessionTwoParams.CollectionParams[].CollectionParamTwo( {value.CollectionParamTwo} )" );
        }

        // Actualizamos la instancia local con los datos recibidos
        s_appSettings = currentAppSettings;

        // Confirmamos las secciones que se desea actualizar
        if( ( s_sessionsToSave & ConfigFileSession.LoggingParams ) == ConfigFileSession.LoggingParams )
        {
            ArgumentNullException.ThrowIfNull( s_appSettings.Logging, nameof( s_appSettings.Logging ) );
            currentAppSettings.Logging.LogLevel.JsonConfigHelper = "Warning";
            currentAppSettings.Logging.EventLog!.LogLevel!.JsonConfigHelper = "Warning";
        }

        if( ( s_sessionsToSave & ConfigFileSession.SessionOneParams ) == ConfigFileSession.SessionOneParams )
        {
            ArgumentNullException.ThrowIfNull( s_appSettings.ServiceParams, nameof( s_appSettings.ServiceParams ) );
            ArgumentNullException.ThrowIfNull( s_appSettings.ServiceParams.SessionOneParams, nameof( s_appSettings.ServiceParams.SessionOneParams ) );

            currentAppSettings.ServiceParams.SessionOneParams.LogFileLocation = "C:\\Temp\\SessionOneLog\\";
        }

        if( ( s_sessionsToSave & ConfigFileSession.SessionTwoParams ) == ConfigFileSession.SessionTwoParams )
        {
            ArgumentNullException.ThrowIfNull( s_appSettings.ServiceParams, nameof( s_appSettings.ServiceParams ) );
            ArgumentNullException.ThrowIfNull( s_appSettings.ServiceParams.SessionTwoParams, nameof( s_appSettings.ServiceParams.SessionTwoParams ) );

            currentAppSettings.ServiceParams.SessionTwoParams.LogFileLocation = "C:\\Temp\\SessionTwoLog\\";
            currentAppSettings.ServiceParams.SessionTwoParams.CollectionParams.Add( new CollectionSettings
            {
                CollectionParamOne = "OneDataCollection",
                CollectionParamTwo = "TwoDataCollection"
            } );
        }

        if( ( s_sessionsToSave & ConfigFileSession.ServiceParams ) == ConfigFileSession.ServiceParams )
        {
            ArgumentNullException.ThrowIfNull( s_appSettings.ServiceParams, nameof( s_appSettings.ServiceParams ) );
            currentAppSettings.ServiceParams.LogFileLocation = "C:\\Temp\\ServiceParamsLog\\";
        }

        return Result.Ok( currentAppSettings );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    public static void Main( string[] args )
    {
        // Definimos las secciones del archivo de configuración que deseamos actualizar
        s_sessionsToSave = ConfigFileSession.SessionOneParams | ConfigFileSession.SessionTwoParams |
            ConfigFileSession.ServiceParams | ConfigFileSession.LoggingParams;

        var settingsFilePath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "appsettings.json" );

        var jsonOpers = new JsonOperations<JsonConfigFile>( settingsFilePath );
        var saveResult = jsonOpers.SaveDataInConfigFile( UpdateJsonAppSettings );

        if( saveResult.IsSuccess )
        {
            var loadResult = jsonOpers.LoadConfigFileData();

            if( loadResult.IsSuccess )
            {
                // Como la lectura fue exitosa, obtenemos los datos leídos
                s_appSettings = loadResult.Value;

                // Validamos que las diferentes secciones del archivo de configuración esten completas
                ArgumentNullException.ThrowIfNull( s_appSettings, nameof( s_appSettings ) );
                ArgumentNullException.ThrowIfNull( s_appSettings.Logging, nameof( s_appSettings.Logging ) );

                ArgumentNullException.ThrowIfNull( s_appSettings.ServiceParams,
                    nameof( s_appSettings.ServiceParams ) );

                ArgumentNullException.ThrowIfNull( s_appSettings.ServiceParams.SessionOneParams,
                    nameof( s_appSettings.ServiceParams.SessionOneParams ) );

                ArgumentNullException.ThrowIfNull( s_appSettings.ServiceParams.SessionTwoParams,
                    nameof( s_appSettings.ServiceParams.SessionTwoParams ) );

                ArgumentNullException.ThrowIfNull( s_appSettings.ServiceParams.SessionTwoParams.CollectionParams,
                    nameof( s_appSettings.ServiceParams.SessionTwoParams.CollectionParams ) );

                Console.WriteLine();
                Console.WriteLine( "Datos después de actualizar la configuración =>\r\n" );
                Console.WriteLine( $"Logging.LogLevel.JsonConfigHelper( {s_appSettings.Logging.LogLevel!.JsonConfigHelper} )" );
                Console.WriteLine( $"Logging.EventLog.LogLevel.JsonConfigHelper( {s_appSettings.Logging.EventLog!.LogLevel!.JsonConfigHelper} )" );
                Console.WriteLine( $"ServiceParams.LogFileLocation( {s_appSettings.ServiceParams.LogFileLocation} )" );
                Console.WriteLine( $"ServiceParams.SessionOneParams.LogFileLocation( {s_appSettings.ServiceParams.SessionOneParams.LogFileLocation} )" );
                Console.WriteLine( $"ServiceParams.SessionTwoParams.LogFileLocation( {s_appSettings.ServiceParams.SessionTwoParams.LogFileLocation} )" );

                foreach( var value in s_appSettings.ServiceParams.SessionTwoParams.CollectionParams )
                {
                    Console.WriteLine( $"ServiceParams.SessionTwoParams.CollectionParams[].CollectionParamOne( {value.CollectionParamOne} )" );
                    Console.WriteLine( $"ServiceParams.SessionTwoParams.CollectionParams[].CollectionParamTwo( {value.CollectionParamTwo} )" );
                }
            }
            else
            {
                Console.WriteLine( loadResult.Reasons[ 0 ] );
            }
        }
        else
        {
            Console.WriteLine( saveResult.Reasons[ 0 ] );
        }

        Console.ReadKey();
    }
}
