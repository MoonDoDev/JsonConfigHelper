# JsonConfigHelper - Lectura y escritura agnóstica de archivos JSON

Esta es una clase que nos muestra un ejemplo de cómo interactuar con los archivos de configuración en formato JSON de forma agnóstica, es decir, independiente de la estructura que tenga el archivo JSON, y simplemente suministrando una clase que represente la estructura de dicho archivo, nos permitirá leerlo y actualizarlo.

Adicional a lo anterior, estamos mostrando cómo crear nuestros propios atributos, para aplicarlos en las propiedades de una clase/record, y que para nuestro ejemplo, nos permitirá indicar que el valor de la propiedad está y debe ser cifrado. 

El atributo que estamos creando se llama **`[PropertyIsCoded]`**, el cual estamos aplicando a las propiedades de la clase/record **`CollectionSettings`**. Y para validar la existencia de dicho atributo, creamos el método estático **`IsCoded()`** en la clase **`JsonProperty`**.

Este proyecto esta compuesto por la clase **`JsonConfigHelper`**, y un programa Demo **`AppConfigTest`** que nos muestra cómo consumir la clase. El proyecto está estructurado de la siguiente manera:
```
AppConfigTest \
| Settings \
  | CollectionSettings  // Sección "CollectionParams" de la sessión "SessionTwoParams".
  | JsonConfigFile      // Estructura del archivo JSON.
  | ServiceSettings     // Sección "ServiceParams" del archivo JSON.
  | SessionOneSettings  // Sección "SessionOneParams" de la sección "ServiceParams".
  | SessionTwoSettings  // Sección "SessionTwoParams" de la sección "ServiceParams".
| AesCipherText         // Clase utilitaria para el cifrado.
| EncryptionService     // Clase principal utilizada para el cifrado.
| Program               // Clase estática con el método Main del programa DEMO.
| appsettings.json      // Archivo de configuración de la App.
JsonConfigHelper \
| JsonAttributes        // Definición del atributo para las propiedades
| JsonExtensions        // Definición de las Extensiones requeridas.
| JsonOperations        // Implementación de la lectura/escritura del archivo JSON.
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
        "CipherSeed": "86AAE9A9939F4E59A863C3FD8211FB65",
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
                    "CollectionParamOne": "ADGKQKk+Q+NNH6LcWmmHIRSzlV+ve+FmIfHiN4qMNRXIric=",
                    "CollectionParamTwo": "Two-One"
                },
                {
                    "CollectionParamOne": "Ribi1hx3IxChdi+T8b21S2w9oJTL8DSqX/laNAyiEuWmVN4=",
                    "CollectionParamTwo": "Two-Two"
                },
                {
                    "CollectionParamOne": "/p6A0+WsHjpqBM76JMeeBZ0pv17CB7IVA5EQrDK3I6SskS7llA==",
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
        "CipherSeed": "86AAE9A9939F4E59A863C3FD8211FB65",
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
                    "CollectionParamOne": "ADGKQKk+Q+NNH6LcWmmHIRSzlV+ve+FmIfHiN4qMNRXIric=",
                    "CollectionParamTwo": "Two-One"
                },
                {
                    "CollectionParamOne": "Ribi1hx3IxChdi+T8b21S2w9oJTL8DSqX/laNAyiEuWmVN4=",
                    "CollectionParamTwo": "Two-Two"
                },
                {
                    "CollectionParamOne": "/p6A0+WsHjpqBM76JMeeBZ0pv17CB7IVA5EQrDK3I6SskS7llA==",
                    "CollectionParamTwo": "Two-Three"
                },
                {
                    "CollectionParamOne": "y+mFZEFAQQIEtWOJTwHDKiq786w0qj51C67VlbHKF8QtOuVxKKi5RmyKzNnz",
                    "CollectionParamTwo": "TwoDataCollection"
                }
            ]
        }
    }
}
```
## Características de la clase JsonConfigHelper?
- [x] Hacemos uso de la característica Generics de C#, para definir una clase con la estructura del archivo JSON que vamos a leer/escribir, a través del parámetro **`<T>`**, y que para nuestro caso sería **`JsonConfigFile`**.

- [x] Creamos, asignamos y validamos atributos personalizados en la clase/record **`CollectionSettings`** para facilitar el manejo de la información sensible, de manera que la podamos cifrar para guardarla y descifrarla para visualizarla.

- [x] Definimos **`ConfigFileSession`** el cuál es de tipo **`enum`** y que operamos como **`[Flags]`** para indicarle a la clase dinámicamente, cuáles de las secciones del archivo JSON necesitamos actualizar.

- [x] Para retornar el resultado de las diferentes operaciones estamos utilizando el tipo **`Result<T>`** de [**FluentResults**](https://www.nuget.org/packages/FluentResults), el cual nos permite retornar un objeto, indicando si hubo éxito o no en la operación, evitando de esta manera lanzar/usar excepciones.

- [x] Por último estamos haciendo uso de los **`delegates`** para permitirle al usuario de la clase definir un método, a modo de **`callback`**, para validar/actualizar los valores de los parámetros del JSON antes de guardarlos en el archivo de configuración.

## Dependencias
```
"FluentResults" Version="3.16.0"
"Newtonsoft.Json" Version="13.0.3"
```
---------
[**YouTube**](https://www.youtube.com/@hectorgomez-backend-dev/featured) - 
[**LinkedIn**](https://www.linkedin.com/in/hectorgomez-backend-dev/) - 
[**GitHub**](https://github.com/MoonDoDev/JsonConfigHelper)
