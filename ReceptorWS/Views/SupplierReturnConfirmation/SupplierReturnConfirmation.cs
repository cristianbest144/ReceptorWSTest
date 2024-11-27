namespace PresentationLayer.Views
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "recibirConfDevolucionProveedor")]
    public class recibirConfDevolucionProveedor
    {
        [XmlElement(ElementName = "message")]
        public Message Message { get; set; }
        [XmlElement(ElementName = "confDevolucionProveedor")]
        public ConfDevolucionProveedor ConfDevolucionProveedor { get; set; }
        [XmlElement(ElementName = "confLineaDevolucionProveedor")]
        public confLineaDevolucionProveedor ConfLineaDevolucionProveedor { get; set; }
    }

    [XmlRoot("confDevolucionProveedor")]
    [XmlType(TypeName = "ConfDevolucionProveedor")]
    public class ConfDevolucionProveedor
    {
        [XmlElement(ElementName = "sitcab")]
        public string Sitcab { get; set; }
        [XmlElement(ElementName = "numdoc")]
        public string Numdoc { get; set; }
        [XmlElement(ElementName = "almace")]
        public string Almace { get; set; }
        [XmlElement(ElementName = "accion")]
        public string Accion { get; set; }
        [XmlElement(ElementName = "proext")]
        public string Proext { get; set; }
        [XmlElement(ElementName = "fecha")]
        public string Fecha { get; set; }
        [XmlElement(ElementName = "provee")]
        public string Provee { get; set; }
        [XmlElement(ElementName = "fecdev")]
        public string Fecdev { get; set; }
        [XmlElement(ElementName = "codext")]
        public string Codext { get; set; }
        [XmlElement(ElementName = "codigo")]
        public string Codigo { get; set; }
        [XmlElement(ElementName = "propie")]
        public string Propie { get; set; }

       

    }

    [XmlRoot("confLineaDevolucionProveedor")]
    public class confLineaDevolucionProveedor
    {
        [XmlElement("ConfLineaDevolucionProveedor")]
        public List<ConfLineaDevolucionProveedor> ConfLineaDevolucionProveedor { get; set; }
    }
    [XmlType(TypeName = "ConfLineaDevolucionProveedor")]
    public class ConfLineaDevolucionProveedor
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
        [XmlElement(ElementName = "causad")]
        public string Causad { get; set; }
        [XmlElement(ElementName = "cantid")]
        public string Cantid { get; set; }
        [XmlElement(ElementName = "feccad")]
        public string Feccad { get; set; }
        [XmlElement(ElementName = "codlin")]
        public string Codlin { get; set; }
        [XmlElement(ElementName = "codext")]
        public string Codext { get; set; }
        [XmlElement(ElementName = "codigo")]
        public string Codigo { get; set; }
        [XmlElement(ElementName = "accion")]
        public string Accion { get; set; }
        [XmlElement(ElementName = "aptnap")]
        public string Aptnap { get; set; }
        [XmlElement(ElementName = "artpv1")]
        public string Artpv1 { get; set; }
        [XmlElement(ElementName = "artpv2")]
        public string Artpv2 { get; set; }
        [XmlElement(ElementName = "fecha")]
        public string Fecha { get; set; }

        [XmlElement(ElementName = "lotefa")]
        public string LoteFa { get; set; }
    }
}
