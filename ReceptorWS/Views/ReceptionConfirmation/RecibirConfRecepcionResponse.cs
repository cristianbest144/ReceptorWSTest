using System.Xml.Serialization;

namespace PresentationLayer.Views
{
    [XmlRoot(ElementName = "recibirConfRecepcionResult")]
    public class RecibirConfRecepcionResult
    {
        [XmlElement(ElementName = "codigo")]
        public int Codigo { get; set; }

        [XmlElement(ElementName = "mensaje")]
        public string Mensaje { get; set; }
    }
}
