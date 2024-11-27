namespace PresentationLayer.Views
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "recibirConfCambioSituacionLogica")]
    public class recibirConfCambioSituacionLogica
    {
        [XmlElement(ElementName = "message")]
        public Message Message { get; set; }
        [XmlElement(ElementName = "confCambioSituacionLogica")]
        public ConfCambioSituacionLogica ConfCambioSituacionLogica { get; set; }
    }
    [XmlRoot(ElementName = "confCambioSituacionLogica")]
    [XmlType(TypeName = "ConfCambioSituacionLogica")]
    public class ConfCambioSituacionLogica
    {
        [XmlElement(ElementName = "stviej")]
        public string Stviej { get; set; }
        [XmlElement(ElementName = "stnuev")]
        public string Stnuev { get; set; }
        [XmlElement(ElementName = "almace")]
        public string Almace { get; set; }
        [XmlElement(ElementName = "accion")]
        public string Accion { get; set; }
        [XmlElement(ElementName = "cantid")]
        public string Cantid { get; set; }
        [XmlElement(ElementName = "coment")]
        public string Coment { get; set; }
        [XmlElement(ElementName = "varlog")]
        public string Varlog { get; set; }
        [XmlElement(ElementName = "varia1")]
        public string Varia1 { get; set; }
        [XmlElement(ElementName = "varia2")]
        public string Varia2 { get; set; }
        [XmlElement(ElementName = "feccad")]
        public string Feccad { get; set; }
        [XmlElement(ElementName = "lotefa")]
        public string Lotefa { get; set; }
        [XmlElement(ElementName = "feccam")]
        public string Feccam { get; set; }
        [XmlElement(ElementName = "propie")]
        public string Propie { get; set; }
        [XmlElement(ElementName = "articu")]
        public string Articu { get; set; }
        [XmlElement(ElementName = "artpro")]
        public string Artpro { get; set; }
        [XmlElement(ElementName = "artpv1")]
        public string Artpv1 { get; set; }
        [XmlElement(ElementName = "artpv2")]
        public string Artpv2 { get; set; }
        [XmlElement(ElementName = "artpvl")]
        public string Artpvl { get; set; }
    }

}
