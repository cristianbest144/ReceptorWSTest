using System.Xml.Serialization;

namespace PresentationLayer.Views
{
    [XmlRoot("recibirConfDevolucionClienteResult")]
    public class RecibirConfDevolucionClienteResult
    {
        [XmlElement("codigo")]
        public int Codigo { get; set; }
        [XmlElement("mensaje")]
        public string Mensaje { get; set; }
    }
}
