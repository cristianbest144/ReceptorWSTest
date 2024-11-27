using BusinessLogicLayer.Dto;
using DataAccessLayer.Entites;
using Shared;
using Shared.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BusinessLogicLayer.Services.Common
{
    public class SiesaPlainConnector
    {
        private readonly SettingStorageServices _settingStore;
        public SiesaPlainConnector()
        {
            _settingStore = new SettingStorageServices();
        }
        public SiesaResponse SendPlan(List<string> listLinea, bool savePlane, string planeName, int companyId, int siesaCode, List<Configuration> _settingStore, string successMessage = "")
        {
           var configuration = new ConfigurationDto
            {
                ws_siesa_conexion = _settingStore.FirstOrDefault(t=> t.Name == Constants.WS_SIESA_CONEXION && t.Id_Cia == companyId).Value,
                user_siesa = _settingStore.FirstOrDefault(t => t.Name == Constants.WS_USER_SIESA && t.Id_Cia == companyId).Value,
                pass_siesa = _settingStore.FirstOrDefault(t => t.Name == Constants.WS_PASS_SIESA && t.Id_Cia == companyId).Value,
                url = _settingStore.FirstOrDefault(t => t.Name == Constants.URLSIESAREQUEST && t.Id_Cia == companyId).Value,
                action = _settingStore.FirstOrDefault(t => t.Name == Constants.ACTIONSIESAREQUEST && t.Id_Cia == companyId).Value,
            };

            XDocument xmlDocument = GetXmlPlan(listLinea, companyId, siesaCode, configuration);
            if (savePlane)
                xmlDocument.SaveXmlDocument(planeName);
            ////return new SiesaResponse
            //{
            //     Success=true,
            //     MessageDescription="Ok",
            //     ResponseMessage="Ok"
            //};

            return ConnectorSiesa(xmlDocument, companyId, configuration, successMessage);
        }

        private SiesaResponse ConnectorSiesa(XDocument xmlDocument, int companyId, ConfigurationDto configuration, string successMessage = "")
        {
            SiesaResponse result = new SiesaResponse()
            {
                Type = ResponseType.EnumTipoErrorSinError,
                Success = false,
            };

            try
            {
                string url = configuration.url;
                string action = configuration.action;
                var SoapService = new SoapCollection();
                var soap = SoapService.CallWebServiceAsync(xmlDocument, companyId, configuration);
                var resultSiesa = XDocument.Parse(soap);
                result.MessageDescription = GetResponseDetails(soap, out int status);
                result.Success = status == 0 ? true : false;
                result.Type = (ResponseType)status;
                result.ResponseMessage = GetResponseMessage(result.Type, successMessage);
            }
            catch (Exception ex)
            {
                result.MessageDescription = ex.Message;
            }

            return result;
        }

        private string GetResponseDetails(string strReturnStatus, out int statusError)
        {
            XDocument doc = XDocument.Parse(strReturnStatus);

            statusError = Convert.ToInt32(doc.Descendants().Where(x => x.Name.LocalName == "printTipoError").FirstOrDefault().Value);

            var val = doc.Descendants().Where(x => x.Name.LocalName == "diffgram").FirstOrDefault().Value;
            if (!string.IsNullOrEmpty(val))
                val = string.Join("\r\n", doc.Descendants()
                                            .Where(x => x.Name.LocalName == "diffgram")
                                            .FirstOrDefault()
                                            .Descendants()
                                                    .Where(x => x.Name.LocalName == "Table")
                                                    .Select(x => x.Value)
                                                    .ToList());

            return val;
        }

        private XDocument GetXmlPlan(List<string> listLinea, int companyId, int siesaCode, ConfigurationDto configuration)
        {
            //string ws_siesa_conexion = (await _settingStore.GetSettingOrNullAsync(null, null, AppQDMConsts.ws_siesa_conexion)).Value;
            //string user_siesa = (await _settingStore.GetSettingOrNullAsync(null, null, AppQDMConsts.ws_user_siesa)).Value;
            //string pass_siesa = (await _settingStore.GetSettingOrNullAsync(null, null, AppQDMConsts.ws_pass_siesa)).Value;

            string ws_siesa_conexion = configuration.ws_siesa_conexion;
            string user_siesa = configuration.user_siesa;
            string pass_siesa = configuration.pass_siesa;

            PlaneSiesaDto planeSiesa = new PlaneSiesaDto
            {
                NombreConexion = ws_siesa_conexion,
                IdCia = siesaCode,
                Usuario = user_siesa,
                Clave = pass_siesa,
                Linea = listLinea
            };

            XDocument xmlDocument = GetXml(planeSiesa);
            return xmlDocument;
        }

        private XDocument GetXml(PlaneSiesaDto plano)
        {
            XDocument doc =
                new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                        new XElement("Importar",
                        new XElement("NombreConexion", plano.NombreConexion),
                        new XElement("IdCia", plano.IdCia),
                        new XElement("Usuario", plano.Usuario),
                        new XElement("Clave", plano.Clave),
                        new XElement("Datos",
                                plano.Linea.Select(x => new XElement("Linea", x))
                            )
                        )
                    );
            return doc;
        }

        private string GetResponseMessage(ResponseType type, string successMessage = "")
        {
            switch (type)
            {
                case ResponseType.EnumTipoErrorSinError:
                    return successMessage;
                case ResponseType.EnumTipoErrorRevisarLog:
                    return $"Código de error 1: Los registros se enviaron pero se presentaron errores, revisar el log de importaciones de Siesa";
                case ResponseType.EnumTipoErrorFaltanParametros:
                    return $"Código de error 2: Hace falta algún parámetro";
                case ResponseType.EnumTipoErrorUsuario:
                    return $"Código de error 3: El usuario o la contraseña que se ingresó en los parámetros es incorrecto";
                case ResponseType.EnumTipoErrorServidores:
                    return $"Código de error 4: Servidores";
                case ResponseType.EnumTipoErrorBD:
                    return $"Código de error 5: La base de datos no existe o están ingresándole un parámetro erróneo";
                case ResponseType.EnumTipoErrorArchivoNoExiste:
                    return $"Código de error 6: La base de datos no existe o están ingresándole un parámetro erróneo";
                case ResponseType.EnumTipoErrorArchivoNoValido:
                    return $"Código de error 7: El archivo que se está especificando en la ruta de los parámetros no es valido";
                case ResponseType.EnumTipoErrorTablaNoValida:
                    return $"Código de error 8: Hay un problema con la tabla en la base de datos donde se ingresaran los archivos";
                case ResponseType.EnumTipoErrorCiaNoValida:
                    return $"Código de error 9: La compañía que se ingresó en los parámetros no es válida";
                case ResponseType.EnumTipoErrorWindowsDesconocido:
                    return $"Código de error 10: Error desconocido";
                case ResponseType.EnumTipoErrorOtro:
                    return $"Código de error 99";
                default:
                    return $"Error";
            }
        }
    }
}
