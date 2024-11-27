namespace PresentationLayer.Views
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "recibirConfRegularizacionStock")]
    public class recibirConfRegularizacionStock
    {
        [XmlElement(ElementName = "message", Namespace = "")]
        public Message Message { get; set; }
        [XmlElement(ElementName = "RegularizacionStock", Namespace = "")]
        public ConfRegularizacionStock ConfRegularizacionStock { get; set; }
    }
    [XmlRoot(ElementName = "confRegularizacionStock", Namespace = "")]
    [XmlType(TypeName = "RegularizacionStock")]
    public class ConfRegularizacionStock
    {
        [XmlElement(ElementName = "varlog")]
        public string Varlog { get; set; }
        [XmlElement(ElementName = "fecaju")]
        public string Fecaju { get; set; }
        [XmlElement(ElementName = "varia1")]
        public string Varia1 { get; set; }
        [XmlElement(ElementName = "varia2")]
        public string Varia2 { get; set; }
        [XmlElement(ElementName = "propie")]
        public string Propie { get; set; }
        [XmlElement(ElementName = "causal")]
        public string Causal { get; set; }
        [XmlElement(ElementName = "artpvl")]
        public string Artpvl { get; set; }
        [XmlElement(ElementName = "cantid")]
        public string Cantid { get; set; }
        [XmlElement(ElementName = "fecha")]
        public string Fecha { get; set; }
        [XmlElement(ElementName = "articu")]
        public string Articu { get; set; }
        [XmlElement(ElementName = "artpro")]
        public string Artpro { get; set; }
        [XmlElement(ElementName = "bulrec")]
        public string Bulrec { get; set; }
        [XmlElement(ElementName = "signoa")]
        public string Signoa { get; set; }
        [XmlElement(ElementName = "paleta")]
        public string Paleta { get; set; }
        [XmlElement(ElementName = "feccad")]
        public string Feccad { get; set; }
        [XmlElement(ElementName = "codaju")]
        public string Codaju { get; set; }
        [XmlElement(ElementName = "almace")]
        public string Almace { get; set; }
        [XmlElement(ElementName = "accion")]
        public string Accion { get; set; }
        [XmlElement(ElementName = "artpv1")]
        public string Artpv1 { get; set; }
        [XmlElement(ElementName = "artpv2")]
        public string Artpv2 { get; set; }
        [XmlElement(ElementName = "codmov")]
        public string Codmov { get; set; }
        [XmlElement(ElementName = "sitlog")]
        public string Sitlog { get; set; }

        [XmlElement(ElementName = "lotefa")]
        public string Lotefa { get; set; }
    }
}
