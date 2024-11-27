using System.Xml.Serialization;

namespace PresentationLayer.Views
{
    [XmlRoot("recibirConfCambioSituacionLogicaResult")]
    public class RecibirConfCambioSituacionLogicaResult
    {
        [XmlElement("codigo")]
        public int Codigo { get; set; }
        [XmlElement("mensaje")]
        public string Mensaje { get; set; }
    }
}
