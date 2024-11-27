using System.Xml.Serialization;

namespace PresentationLayer.Views
{
    [XmlRoot("recibirConfCargaCamionResult")]
    public class RecibirConfCargaCamionResult
    {
        [XmlElement("codigo")]
        public int Codigo { get; set; }
        [XmlElement("mensaje")]
        public string Mensaje { get; set; }
    }
}
