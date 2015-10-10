# Refactor

### Constructor/Propiedades de Logger
Es conveniente que en vez de pasarle distintos valores booleanos para determinar qué tipo de mensaje loggear y cómo, que se creen dos tipos de enumerados para indicarlos.

Esto hace que la creación del objeto sea menos complicada (que elimina la posibilidad de cometer errores por poner el argumento incorrecto en "true") y sea más mantenible (es más sencillo y menos riesgoso agregar una nueva entrada al enumerado que añadir un nuevo argumento al constructor).

Esto además elimina la siguiente validación en el método para loggear el mensaje:

```
if (!_logToConsole && !_logToFile && !LogToDatabase)
{
  throw new Exception("Invalid configuration");
}

if ((!_logError && !_logMessage && !_logWarning) || (!message && !warning && !error))
{
  throw new Exception("Error or Warning or Message must be specified");
}
```

### Método para loggear mensaje
Como con el constructor, conviene pasar un enumerado para denotar qué tipo de mensaje se quiere loggear.

Otro error es usar el mismo condicional en tres lugares distintos para settear distintas variables en vez de hacerlas dentro del mismo bloque IF:

```
if (type == LogType.Message && _logMessage)
{
    // Todo lo que sea para el caso Mensaje
}
if (type == LogType.Error && _logError)
{
    // Todo lo que sea para el caso Error
}
if (type == LogType.Warning && _logWarning)
{
    // Todo lo que sea para el tipo Warning
}
```

### Instrucciones redundantes
La siguiente instrucción se encontraba repetida en cada uno de los IF, por lo que puede escribirse una sóla vez cuando se determina que existe el archivo de loggeo

```
l = l + DateTime.Now.ToShortDateString() + messageS;
```

### Uso del valor de un enumerado para indicar Tipo de Mensaje
Al usar un enumerado para indicar el tipo de mensaje a loggear, la variable "t" tampoco es necesaria, porque puede pasarse el valor del argumento de tipo de mensaje y asociarlo con el valor correspondiente que tiene "t" para cada uno en la declaración del enumerado.

### Catcheo de la persistencia a base de datos
Es necesario catchear las instrucciones para la persistencia en base de datos, de modo de informar en caso de que no se haya podido hacer esta.

### Usar AppendText para escribir en el archivo
En vez de fijarse si existe el archivo, leer sus contenidos en un string y luego appendear el nuevo mensaje en ese string y volver a sobre-escribir el archivo, es preferible directamente appendear el nuevo mensaje con AppendText. En caso que no exista el archivo, el mismo método lo crea.
