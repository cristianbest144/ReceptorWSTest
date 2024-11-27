namespace BusinessLogicLayer.Services.Common
{
    public class SiesaResponse
    {
        public bool Success { set; get; }
        public string MessageDescription { set; get; }
        public ResponseType Type { set; get; }
        public string ResponseMessage { get; set; }
    }

    public enum ResponseType
    {
        EnumTipoErrorSinError = 0, //Los registros pasaron correctamente
                                   //(ORDEN DE COMPRA GENERADA = OCI_49)

        EnumTipoErrorRevisarLog = 1, //Los registros se enviaron pero presentan errores hay q revisar el log de importaciones de ENTERPRISE
        //(Código de error 1: Los registros se enviaron pero se presentaron errores, revisar el log de importaciones de Siesa)

        EnumTipoErrorFaltanParametros = 2, //Hace falta algún parámetro en el archivo .BAT
        //(Código de error 2: Hace falta algún parámetro)

        EnumTipoErrorUsuario = 3,
        //El usuario o la contraseña que se ingresó en los parámetros del .BAT esta erróneo
        //(Código de error 2: El usuario o la contraseña que se ingresó en los parámetros es incorrecto)

        EnumTipoErrorServidores = 4,
        //El error puede ser que el “Impodatos.exe” tiene una versión diferente a la del ejecutable de ENTERPRISE, o también puede ser que estén ejecutando el archivo .BAT desde una máquina que no tiene instalado ENTERPRISE o por lo menos su cliente.

        EnumTipoErrorBD = 5,
        //La base de datos no existe o están ingresándole un parámetro erróneo a la hora de especificar la conexión.

        EnumTipoErrorArchivoNoExiste = 6,
        //El archivo que se está especificando en la ruta de los parámetros del .BAT no existe

        EnumTipoErrorArchivoNoValido = 7,
        //El archivo que se está especificando en la ruta de los parámetros del .BAT no es valido

        EnumTipoErrorTablaNoValida = 8,
        //Hay un problema con la tabla en la base de datos donde se ingresaran los archivos

        EnumTipoErrorCiaNoValida = 9,
        //La compañía que se ingresó en los parámetros del .BAT no es valida

        EnumTipoErrorWindowsDesconocido = 10,
        //Error desconocido

        EnumTipoErrorOtro = 99,// = 99
        //Error de tipo diferente a los anteriores
    }
}
