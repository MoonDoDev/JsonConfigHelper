# JsonConfigHelper - Lectura y escritura agnóstica de archivos JSON

Esta es una clase que nos ayuda a interactuar con los archivos de configuración en formato JSON de forma agnóstica, es decir, independiente de la estructura que tenga el archivo JSON, y simplemente suministrando una clase que represente la estructura de dicho archivo, nos permitirá leerlo y actualizarlo.

Este proyecto esta compuesto por la clase `JsonConfigHelper`, y un programa Demo `AppConfigTest` que nos muestra cómo consumir la clase. El proyecto está estructurado de la siguiente manera:

```
AppConfigTest \
| Settings \
  | JsonConfigFile      // Representa la estructura del archivo JSON.
  | ServiceSettings     // Representa la sección "ServiceParams" del archivo JSON.
  | SessionOneSettings  // Representa la sección "SessionOneParams" de la sección "ServiceParams".
  | SessionTwoSettings  // Representa la sección "SessionTwoParams" de la sección "ServiceParams".
  | CollectionSettings  // Representa la sección "CollectionParams" de la sessión "SessionTwoParams".
| Program               // Clase estática con el método Main del programa DEMO.
JsonConfigHelper \
| JsonExtensions        // Clase estática con la definición de las Extensiones requeridas.
| JsonOperations        // Clase con la implementación de la lectura/escritura del archivo JSON.
```

Para efectos de demostración se ha creado en el programa Demo, un archivo JSON de configuración **appsettings.json** con la siguiente estructura:

```
{
    "Logging": {
        "LogLevel": {
            "Default": "Warning",
            "JsonConfigHelper": "Trace"
        },
        "EventLog": {
            "SourceName": "JSON Configuration Helper",
            "LogName": "JSON Configuration Helper Events",
            "LogLevel": {
                "Default": "Warning",
                "JsonConfigHelper": "Trace"
            }
        }
    },
    "AllowedHosts": "*",
    "ServiceParams": {
        "CreateLogFile": true,
        "LogFileLocation": "C:\\Temp\\",
        "SessionOneParams": {
            "CreateLogFile": false,
            "LogFileLocation": "C:\\Temp\\"
        },
        "SessionTwoParams": {
            "CreateLogFile": false,
            "LogFileLocation": "C:\\Temp\\",
            "CollectionParams": [
                {
                    "CollectionParamOne": "One-One",
                    "CollectionParamTwo": "Two-One"
                },
                {
                    "CollectionParamOne": "One-Two",
                    "CollectionParamTwo": "Two-Two"
                },
                {
                    "CollectionParamOne": "One-Three",
                    "CollectionParamTwo": "Two-Three"
                }
            ]
        }
    }
}
```

Y al ejecutar exitosamente el programa DEMO, el archivo JSON de configuración **appsettings.json**, quedaría con la siguiente estructura y valores:

```
{
    "Logging": {
        "LogLevel": {
            "Default": "Warning",
            "JsonConfigHelper": "Warning"
        },
        "EventLog": {
            "SourceName": "JSON Configuration Helper",
            "LogName": "JSON Configuration Helper Events",
            "LogLevel": {
                "Default": "Warning",
                "JsonConfigHelper": "Warning"
            }
        }
    },
    "AllowedHosts": "*",
    "ServiceParams": {
        "CreateLogFile": true,
        "LogFileLocation": "C:\\Temp\\ServiceParamsLog\\",
        "SessionOneParams": {
            "CreateLogFile": false,
            "LogFileLocation": "C:\\Temp\\SessionOneLog\\"
        },
        "SessionTwoParams": {
            "CreateLogFile": false,
            "LogFileLocation": "C:\\Temp\\SessionTwoLog\\",
            "CollectionParams": [
                {
                    "CollectionParamOne": "One-One",
                    "CollectionParamTwo": "Two-One"
                },
                {
                    "CollectionParamOne": "One-Two",
                    "CollectionParamTwo": "Two-Two"
                },
                {
                    "CollectionParamOne": "One-Three",
                    "CollectionParamTwo": "Two-Three"
                },
                {
                    "CollectionParamOne": "OneDataCollection",
                    "CollectionParamTwo": "TwoDataCollection"
                }
            ]
        }
    }
}
```

## Características de la clase JsonConfigHelper

- [x]  Hacemos uso de la característica Generics de C#, para definir una clase con la estructura del archivo JSON que vamos a leer/escribir, a través del parámetro `<T>`, y que para nuestro caso sería `JsonConfigFile`.

- [x]  Definimos `ConfigFileSession` el cuál es de tipo `enum` y que operamos como `[Flags]` para indicarle a la clase dinámicamente, cuáles de las secciones del archivo JSON necesitamos guardar.

- [x]  Para retornar el resultado de las diferentes operaciones estamos utilizando el tipo `Result<T>` de [**FluentResults**](https://www.nuget.org/packages/FluentResults), el cual nos permite retornar un objeto, indicando si hubo éxito o no en la operación, evitando de esta manera lanzar/usar excepciones.

- [x]  Por último estamos haciendo uso de los **delegates** para permitirle al usuario de la clase definir un método, a modo de **callback**, para validar/actualizar los valores de los parámetros del JSON antes de guardarlos en el archivo de configuración.

## Dependencias

```
"FluentResults" Version="3.16.0"
"Newtonsoft.Json" Version="13.0.3"
```

---------

[**YouTube**](https://www.youtube.com/@hectorgomez-backend-dev/featured) -- 
[**LinkedIn**](https://www.linkedin.com/in/hectorgomez-backend-dev/) -- 
[**GitHub**](https://github.com/MoonDoDev/JsonConfigHelper)
