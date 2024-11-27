using System.Xml.Serialization;

namespace PresentationLayer.Views
{
    [XmlRoot("recibirConfDevolucionProveedorResult")]
    public class RecibirConfDevolucionProveedorResult
    {
        [XmlElement("codigo")]
        public int Codigo { get; set; }
        [XmlElement("mensaje")]
        public string Mensaje { get; set; }
    }
}
