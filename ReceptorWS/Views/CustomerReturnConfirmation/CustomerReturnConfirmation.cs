namespace PresentationLayer.Views
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "recibirConfDevolucionCliente")]
    public class recibirConfDevolucionCliente
    {
        public Message Message { get; set; }
        public ConfDevolucionCliente ConfDevolucionCliente { get; set; }
        public confLineaDevolucionCliente ConfLineaDevolucionCliente { get; set; }
    }

    [XmlRoot(ElementName = "confDevolucionCliente", Namespace ="")]
    [XmlType(TypeName = "ConfDevolucionCliente")]
    public class ConfDevolucionCliente
    {
        [XmlElement(ElementName = "sitcab")]
        public string Sitcab { get; set; }
        [XmlElement(ElementName = "numdoc")]
        public string Numdoc { get; set; }
        [XmlElement(ElementName = "almace")]
        public string Almace { get; set; }
        [XmlElement(ElementName = "accion")]
        public string Accion { get; set; }
        [XmlElement(ElementName = "cliente")]
        public string Cliente { get; set; }
        [XmlElement(ElementName = "fecha")]
        public string Fecha { get; set; }
        [XmlElement(ElementName = "cliext")]
        public string Cliext { get; set; }
        [XmlElement(ElementName = "totbul")]
        public string Totbul { get; set; }
        [XmlElement(ElementName = "codext")]
        public string Codext { get; set; }
        [XmlElement(ElementName = "fecdev")]
        public string Fecdev { get; set; }
        [XmlElement(ElementName = "codigo")]
        public string Codigo { get; set; }
        [XmlElement(ElementName = "propie")]
        public string Propie { get; set; }
    }

    [XmlRoot(ElementName = "confLineaDevolucionCliente")]
    public class confLineaDevolucionCliente
    {
        [XmlElement(ElementName ="ConfLineaDevolucionCliente", Namespace = "")]
      
        public List<ConfLineaDevolucionCliente> ConfLineaDevolucionCliente { get; set; }
    }
    [XmlType(TypeName = "ConfLineaDevolucionCliente")]
    public class ConfLineaDevolucionCliente
    {
        [XmlElement(ElementName = "varlog")]
        public string Varlog { get; set; }
        [XmlElement(ElementName = "varia1")]
        public string Varia1 { get; set; }
        [XmlElement(ElementName = "varia2")]
        public string Varia2 { get; set; }
        [XmlElement(ElementName = "sitlin")]
        public string Sitlin { get; set; }
        [XmlElement(ElementName = "artpvl")]
        public string Artpvl { get; set; }
        [XmlElement(ElementName = "canrea")]
        public string Canrea { get; set; }
        [XmlElement(ElementName = "fecdev")]
        public string Fecdev { get; set; }
        [XmlElement(ElementName = "articu")]
        public string Articu { get; set; }
        [XmlElement(ElementName = "artpro")]
        public string Artpro { get; set; }
        [XmlElement(ElementName = "lotefa")]
        public string Lotefa { get; set; }
        [XmlElement(ElementName = "numbul")]
        public string Numbul { get; set; }
        [XmlElement(ElementName = "causad")]
        public string Causad { get; set; }
        [XmlElement(ElementName = "cantna")]
        public string Cantna { get; set; }
        [XmlElement(ElementName = "feccad")]
        public string Feccad { get; set; }
        [XmlElement(ElementName = "codlin")]
        public string Codlin { get; set; }
        [XmlElement(ElementName = "codigo")]
        public string Codigo { get; set; }
        [XmlElement(ElementName = "accion")]
        public string Accion { get; set; }
        [XmlElement(ElementName = "abonli")]
        public string Abonli { get; set; }
        [XmlElement(ElementName = "artpv1")]
        public string Artpv1 { get; set; }
        [XmlElement(ElementName = "artpv2")]
        public string Artpv2 { get; set; }
        [XmlElement(ElementName = "cantot")]
        public string Cantot { get; set; }
        [XmlElement(ElementName = "fecha")]
        public string Fecha { get; set; }
    }
}
