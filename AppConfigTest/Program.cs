using AppConfigTest.Settings;
using FluentResults;
using JsonConfigHelper;
using System.Text;

namespace AppConfigTest;

/// <summary>
/// 
/// </summary>
public class Program
{
    private const string CONFIG_FILE_NAME = "appsettings.json";

    private static ConfigFileSession s_sessionsToSave = ConfigFileSession.None;
    private static JsonConfigFile s_appSettings = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentAppSettings"></param>
    /// <returns></returns>
    public static Result<JsonConfigFile> UpdateJsonAppSettings( JsonConfigFile currentAppSettings )
    {
        #region Realizamos la validación de cada una de las secciones del archivo de configuración

        ArgumentNullException.ThrowIfNull( currentAppSettings,
            nameof( currentAppSettings ) );

        ArgumentNullException.ThrowIfNull( currentAppSettings.Logging,
            nameof( currentAppSettings.Logging ) );

        ArgumentNullException.ThrowIfNull( currentAppSettings.ServiceParams,
            nameof( currentAppSettings.ServiceParams ) );

        ArgumentNullException.ThrowIfNull( currentAppSettings.ServiceParams.SessionOneParams,
            nameof( currentAppSettings.ServiceParams.SessionOneParams ) );

        ArgumentNullException.ThrowIfNull( currentAppSettings.ServiceParams.SessionTwoParams,
            nameof( currentAppSettings.ServiceParams.SessionTwoParams ) );

        ArgumentNullException.ThrowIfNull( currentAppSettings.ServiceParams.SessionTwoParams.CollectionParams,
            nameof( currentAppSettings.ServiceParams.SessionTwoParams.CollectionParams ) );

        #endregion

        // Instanciamos el servicio de cifrado
        using var cipher = new EncryptionService(
            Encoding.UTF8.GetBytes( currentAppSettings.ServiceParams.CipherSeed! ) );

        #region Enseñamos los datos que contiene actualmente el archivo de configuración

        Console.Clear();
        Console.WriteLine( "Datos actuales antes de cambiar de configuración =>\r\n" );
        Console.WriteLine( $"Logging.LogLevel.JsonConfigHelper( {currentAppSettings.Logging.LogLevel!.JsonConfigHelper} )" );
        Console.WriteLine( $"Logging.EventLog.LogLevel.JsonConfigHelper( {currentAppSettings.Logging.EventLog!.LogLevel!.JsonConfigHelper} )" );
        Console.WriteLine( $"ServiceParams.LogFileLocation( {currentAppSettings.ServiceParams.LogFileLocation} )" );
        Console.WriteLine( $"ServiceParams.SessionOneParams.LogFileLocation( {currentAppSettings.ServiceParams.SessionOneParams.LogFileLocation} )" );
        Console.WriteLine( $"ServiceParams.SessionTwoParams.LogFileLocation( {currentAppSettings.ServiceParams.SessionTwoParams.LogFileLocation} )" );

        var oneIsEncrypted = JsonProperty.IsCoded<CollectionSettings>(
            nameof( CollectionSettings.CollectionParamOne ) );

        var twoIsEncrypted = JsonProperty.IsCoded<CollectionSettings>(
            nameof( CollectionSettings.CollectionParamTwo ) );

        foreach( var value in currentAppSettings.ServiceParams.SessionTwoParams.CollectionParams )
        {
            var oneValue = oneIsEncrypted ?
                cipher.Decrypt( value.CollectionParamOne! ) : value.CollectionParamOne;

            var twoValue = twoIsEncrypted ?
                cipher.Decrypt( value.CollectionParamTwo! ) : value.CollectionParamTwo;

            Console.WriteLine( $"ServiceParams.SessionTwoParams.CollectionParams[].CollectionParamOne( {oneValue} )" );
            Console.WriteLine( $"ServiceParams.SessionTwoParams.CollectionParams[].CollectionParamTwo( {twoValue} )" );
        }

        #endregion

        // Actualizamos la instancia local con los datos recibidos
        s_appSettings = currentAppSettings;

        #region Realizamos las actualizaciones en las secciones requeridas

        if( ( s_sessionsToSave & ConfigFileSession.LoggingParams ) == ConfigFileSession.LoggingParams )
        {
            currentAppSettings.Logging.LogLevel.JsonConfigHelper = "Warning";
            currentAppSettings.Logging.EventLog!.LogLevel!.JsonConfigHelper = "Warning";
        }

        if( ( s_sessionsToSave & ConfigFileSession.SessionOneParams ) == ConfigFileSession.SessionOneParams )
        {
            currentAppSettings.ServiceParams.SessionOneParams.LogFileLocation = "C:\\Temp\\SessionOneLog\\";
        }

        if( ( s_sessionsToSave & ConfigFileSession.SessionTwoParams ) == ConfigFileSession.SessionTwoParams )
        {
            currentAppSettings.ServiceParams.SessionTwoParams.LogFileLocation = "C:\\Temp\\SessionTwoLog\\";

            currentAppSettings.ServiceParams.SessionTwoParams.CollectionParams.Add( new CollectionSettings
            {
                CollectionParamOne = oneIsEncrypted ?
                    cipher.Encrypt( "OneDataCollection" ) : "OneDataCollection",

                CollectionParamTwo = twoIsEncrypted ?
                    cipher.Encrypt( "TwoDataCollection" ) : "TwoDataCollection"
            } );
        }

        if( ( s_sessionsToSave & ConfigFileSession.ServiceParams ) == ConfigFileSession.ServiceParams )
        {
            currentAppSettings.ServiceParams.LogFileLocation = "C:\\Temp\\ServiceParamsLog\\";
        }

        #endregion

        // Retornamos los cambios realizados
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

        var settingsFilePath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, CONFIG_FILE_NAME );
        var jsonOpers = new JsonOperations<JsonConfigFile>( settingsFilePath );
        var saveResult = jsonOpers.SaveDataInConfigFile( UpdateJsonAppSettings );

        if( saveResult.IsSuccess )
        {
            var loadResult = jsonOpers.LoadConfigFileData();

            if( loadResult.IsSuccess )
            {
                // Como la lectura fue exitosa, obtenemos los datos leídos
                s_appSettings = loadResult.Value;
                ArgumentNullException.ThrowIfNull( s_appSettings, nameof( s_appSettings ) );

                Console.WriteLine();
                Console.WriteLine( "Datos después de actualizar la configuración =>\r\n" );
                Console.WriteLine( $"Logging.LogLevel.JsonConfigHelper( {s_appSettings.Logging!.LogLevel!.JsonConfigHelper} )" );
                Console.WriteLine( $"Logging.EventLog.LogLevel.JsonConfigHelper( {s_appSettings.Logging.EventLog!.LogLevel!.JsonConfigHelper} )" );
                Console.WriteLine( $"ServiceParams.LogFileLocation( {s_appSettings.ServiceParams!.LogFileLocation} )" );
                Console.WriteLine( $"ServiceParams.SessionOneParams.LogFileLocation( {s_appSettings.ServiceParams.SessionOneParams!.LogFileLocation} )" );
                Console.WriteLine( $"ServiceParams.SessionTwoParams.LogFileLocation( {s_appSettings.ServiceParams.SessionTwoParams!.LogFileLocation} )" );

                foreach( var value in s_appSettings.ServiceParams.SessionTwoParams.CollectionParams! )
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
    }
}
