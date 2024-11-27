namespace PresentationLayer.Views
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "recibirOrdenFabricacion")]
    public class recibirOrdenFabricacion
    {
        [XmlElement(ElementName = "message")]
        public Message Message { get; set; }
        [XmlElement(ElementName = "confOrdenFabricacion")]
        public OrdenFabricacion confOrdenFabricacion { get; set; }
    }
    [XmlType(TypeName = "confOrdenFabricacion")]
    public class OrdenFabricacion
    {
        [XmlElement(ElementName = "accion")]
        public string accion { get; set; }
        [XmlElement(ElementName = "articu")]
        public string articu { get; set; }
        [XmlElement(ElementName = "artpro")]
        public string artpro { get; set; }
        [XmlElement(ElementName = "artpv1")]
        public string artpv1 { get; set; }
        [XmlElement(ElementName = "artpv2")]
        public string artpv2 { get; set; }
        [XmlElement(ElementName = "artpvl")]
        public string artpvl { get; set; }
        [XmlElement(ElementName = "cantid")]
        public string cantid { get; set; }
        [XmlElement(ElementName = "codgof")]
        public string codgof { get; set; }

        [XmlElement(ElementName = "codofr")]
        public string codofr { get; set; }
        [XmlElement(ElementName = "feccad")]
        public string feccad { get; set; }
        [XmlElement(ElementName = "fecha")]
        public string fecha { get; set; }
        [XmlElement(ElementName = "fecreg")]
        public string fecreg { get; set; }
        [XmlElement(ElementName = "gofext")]
        public string gofext { get; set; }
        [XmlElement(ElementName = "lotefa")]
        public string lotefa { get; set; }
        [XmlElement(ElementName = "ofrext")]
        public string ofrext { get; set; }
        [XmlElement(ElementName = "varia1")]
        public string varia1 { get; set; }
        [XmlElement(ElementName = "varia2")]
        public string varia2 { get; set; }
        [XmlElement(ElementName = "varlog")]
        public string varlog { get; set; }
    }
}
