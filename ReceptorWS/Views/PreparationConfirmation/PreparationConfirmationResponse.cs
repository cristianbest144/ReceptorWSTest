using System.Xml.Serialization;

namespace PresentationLayer.Views
{
    [XmlRoot("recibirConfPreparacionResult")]
    public class RecibirConfPreparacionResult
    {
       
        [XmlElement(ElementName = "codigo")]
        public int Codigo { get; set; }

        [XmlElement(ElementName = "mensaje")]
        public string Mensaje { get; set; }
    }
}
