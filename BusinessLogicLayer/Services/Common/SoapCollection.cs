using BusinessLogicLayer.Dto;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace BusinessLogicLayer.Services.Common
{
    public class SoapCollection
    {
        /*
        Prod
            private static string _url = "";
            private static string _action = "";
        
        Pruebas
            private static string _url = "https://wsdistridulcespruaws.siesacloud.com:8043/WSUNOEE/WSUNOEE.asmx";
            private static string _action = "https://wsdistridulcespruaws.siesacloud.com:8043/WSUNOEE/WFPruebaImportar.aspx";
        */

        private static readonly HttpClient client = new HttpClient();
        public SoapCollection()
        {

        }

        public string CallWebServiceAsync(XDocument xmlDocument, int companyId, ConfigurationDto configuration)
        {
            try
            {
                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(xmlDocument);
                HttpWebRequest webRequest = CreateWebRequest(configuration.url, configuration.action);
                //webRequest.Timeout = int.MaxValue;

                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }
                //InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest).Wait();
                // begin async call to web request.
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
                // suspend this thread until call is complete. You might want to
                // do something usefull here like update your UI.
                asyncResult.AsyncWaitHandle.WaitOne();
                // get the response from the completed web request.
                string soapResult;
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                    }
                    Console.Write(soapResult);
                }
                return soapResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers["SOAPAction"] = action;
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.Timeout = 600000; // timeout de 10 minutos
            webRequest.ReadWriteTimeout = 600000; // timeout de 10 minutos
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope(XDocument data)
        {
            var rawData = HttpUtility.HtmlEncode(data.ToString());
            var s = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tem=\"http://tempuri.org/\"><soapenv:Header/><soapenv:Body><tem:ImportarXML><tem:pvstrDatos>{0}</tem:pvstrDatos><tem:printTipoError>1</tem:printTipoError></tem:ImportarXML></soapenv:Body></soapenv:Envelope>";
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(String.Format(s, rawData));
            return soapEnvelopeDocument;
        }
    }
}
