using System.Xml.Serialization;

namespace PresentationLayer.Views
{
    [XmlRoot("recibirConfRegularizacionStockResult")]
    public class RecibirConfRegularizacionStockResult
    {
        [XmlElement("codigo")]
        public int Codigo { get; set; }
        [XmlElement("mensaje")]
        public string Mensaje { get; set; }
    }
}
