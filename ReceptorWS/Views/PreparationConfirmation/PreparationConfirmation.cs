namespace PresentationLayer.Views
{
    using System.Xml.Serialization;
    using System.Collections.Generic;

    [XmlRoot(ElementName = "recibirConfPreparacion")]
    public class RecibirConfPreparacion
    {
        [XmlElement(ElementName = "message")]
        public Message Message { get; set; }
        [XmlElement(ElementName = "confPreparacion")]
        public ConfPreparacion ConfPreparacion { get; set; }
        [XmlElement(ElementName = "confLineaPrep")]
        public ConfLineaPrep ConfLineaPrep { get; set; }
    }

    [XmlRoot(ElementName = "confPreparacion")]
    [XmlType(TypeName = "ConfPreparacion")]
    public class ConfPreparacion
    {
        [XmlElement(ElementName = "almace")]
        public string Almace { get; set; }
        [XmlElement(ElementName = "totbul")]
        public string Totbul { get; set; }
        [XmlElement(ElementName = "accion")]
        public string Accion { get; set; }
        [XmlElement(ElementName = "fecser")]
        public string Fecser { get; set; }
        [XmlElement(ElementName = "fectra")]
        public string Fectra { get; set; }
        [XmlElement(ElementName = "fecha")]
        public string Fecha { get; set; }
        [XmlElement(ElementName = "propie")]
        public string Propie { get; set; }
        [XmlElement(ElementName = "pedido")]
        public string Pedido { get; set; }
        [XmlElement(ElementName = "sitped")]
        public string Sitped { get; set; }
        [XmlElement(ElementName = "client")]
        public string Client { get; set; }
        [XmlElement(ElementName = "tipped")]
        public string Tipped { get; set; }
        [XmlElement(ElementName = "pedext")]
        public string Pedext { get; set; }
        [XmlElement(ElementName = "usuari")]
        public string Usuari { get; set; }
        [XmlElement(ElementName = "agenci")]
        public string Agenci { get; set; }
        [XmlElement(ElementName = "divped")]
        public string Divped { get; set; }
        [XmlElement(ElementName = "cliext")]
        public string Cliext { get; set; }
    }

    [XmlRoot("confLineaPrep")]
    public class ConfLineaPrep
    {
        [XmlElement("ConfLineaPreparacion")]
        public List<ConfLineaPreparacion> ConfLineaPreparacion { get; set; }
    }
   
    [XmlType(TypeName = "ConfLineaPreparacion")]
    public class ConfLineaPreparacion
    {
        [XmlElement(ElementName = "varlog")]
        public string Varlog { get; set; }
        [XmlElement(ElementName = "varia1")]
        public string Varia1 { get; set; }
        [XmlElement(ElementName = "varia2")]
        public string Varia2 { get; set; }
        [XmlElement(ElementName = "sitlin")]
        public string Sitlin { get; set; }
        [XmlElement(ElementName = "causaf")]
        public string Causaf { get; set; }
        [XmlElement(ElementName = "artpvl")]
        public string Artpvl { get; set; }
        [XmlElement(ElementName = "bulrec")]
        public string Bulrec { get; set; }
        [XmlElement(ElementName = "fecha")]
        public string Fecha { get; set; }
        [XmlElement(ElementName = "articu")]
        public string Articu { get; set; }
        [XmlElement(ElementName = "lote")]
        public string Lote { get; set; }
        [XmlElement(ElementName = "canpes")]
        public string Canpes { get; set; }
        [XmlElement(ElementName = "feccad")]
        public string Feccad { get; set; }
        [XmlElement(ElementName = "canrec")]
        public string Canrec { get; set; }
        [XmlElement(ElementName = "codlin")]
        public string Codlin { get; set; }
        [XmlElement(ElementName = "accion")]
        public string Accion { get; set; }
        [XmlElement(ElementName = "pedext")]
        public string Pedext { get; set; }
        [XmlElement(ElementName = "pedido")]
        public string Pedido { get; set; }
        [XmlElement(ElementName = "artpro")]
        public string Artpro { get; set; }
        [XmlElement(ElementName = "artpv1")]
        public string Artpv1 { get; set; }
        [XmlElement(ElementName = "divped")]
        public string Divped { get; set; }
        [XmlElement(ElementName = "artpv2")]
        public string Artpv2 { get; set; }
        [XmlElement(ElementName = "canped")]
        public string Canped { get; set; }
        [XmlElement(ElementName = "almkit")]
        public string Almkit { get; set; }
        [XmlElement(ElementName = "pprkit")]
        public string Pprkit { get; set; }
    }
}
