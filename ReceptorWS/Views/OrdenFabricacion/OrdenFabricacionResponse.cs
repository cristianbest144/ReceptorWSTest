using System.Xml.Serialization;

namespace PresentationLayer.Views
{
    [XmlRoot("recibirConfOrdenFabricacionResult")]
    public class RecibirConfOrdenFabricacionResult
    {
        [XmlElement("codigo")]
        public int Codigo { get; set; }
        [XmlElement("mensaje")]
        public string Mensaje { get; set; }
    }
}
