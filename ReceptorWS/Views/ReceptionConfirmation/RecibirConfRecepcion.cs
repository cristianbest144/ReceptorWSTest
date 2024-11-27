using System.Collections.Generic;
using System.Xml.Serialization;

namespace PresentationLayer.Views
{
    [XmlRoot(ElementName = "confRecepcion", Namespace = "")]
    public class ConfRecepcion
    {

        [XmlElement(ElementName = "sitcab", Namespace = "")]
        public string Sitcab { get; set; }

        [XmlElement(ElementName = "nument", Namespace = "")]
        public string Nument { get; set; }

        [XmlElement(ElementName = "almace", Namespace = "")]
        public string Almace { get; set; }

        [XmlElement(ElementName = "accion", Namespace = "")]
        public string Accion { get; set; }

        [XmlElement(ElementName = "numpal", Namespace = "")]
        public string Numpal { get; set; }

        [XmlElement(ElementName = "fecha", Namespace = "")]
        public string Fecha { get; set; }

        [XmlElement(ElementName = "pedido", Namespace = "")]
        public string Pedido { get; set; }

        [XmlElement(ElementName = "usuari", Namespace = "")]
        public string Usuari { get; set; }

        [XmlElement(ElementName = "proext", Namespace = "")]
        public string Proext { get; set; }

        [XmlElement(ElementName = "fecfin", Namespace = "")]
        public string Fecfin { get; set; }

        [XmlElement(ElementName = "provee", Namespace = "")]
        public string Provee { get; set; }

        [XmlElement(ElementName = "tipped", Namespace = "")]
        public string Tipped { get; set; }

        [XmlElement(ElementName = "propie", Namespace = "")]
        public string Propie { get; set; }

        [XmlElement(ElementName = "pedext", Namespace = "")]
        public string Pedext { get; set; }

        [XmlElement(ElementName = "albara", Namespace = "")]
        public string Albara { get; set; }
    }

    [XmlRoot(ElementName = "ConfLineaRecepcion", Namespace = "")]
    public class ConfLineaRecepcion
    {

        [XmlElement(ElementName = "varlog", Namespace = "")]
        public string Varlog { get; set; }

        [XmlElement(ElementName = "varia1", Namespace = "")]
        public string Varia1 { get; set; }

        [XmlElement(ElementName = "varia2", Namespace = "")]
        public string Varia2 { get; set; }

        [XmlElement(ElementName = "sitlin", Namespace = "")]
        public string Sitlin { get; set; }

        [XmlElement(ElementName = "artpvl", Namespace = "")]
        public string Artpvl { get; set; }

        [XmlElement(ElementName = "bulrec", Namespace = "")]
        public string Bulrec { get; set; }

        [XmlElement(ElementName = "fecha", Namespace = "")]
        public string Fecha { get; set; }

        [XmlElement(ElementName = "articu", Namespace = "")]
        public string Articu { get; set; }

        [XmlElement(ElementName = "artpro", Namespace = "")]
        public string Artpro { get; set; }

        [XmlElement(ElementName = "nument", Namespace = "")]
        public string Nument { get; set; }

        [XmlElement(ElementName = "cantco", Namespace = "")]
        public string Cantco { get; set; }

        [XmlElement(ElementName = "canpes", Namespace = "")]
        public string Canpes { get; set; }

        [XmlElement(ElementName = "feccad", Namespace = "")]
        public string Feccad { get; set; }

        [XmlElement(ElementName = "codlin", Namespace = "")]
        public string Codlin { get; set; }

        [XmlElement(ElementName = "accion", Namespace = "")]
        public string Accion { get; set; }

        [XmlElement(ElementName = "pedext", Namespace = "")]
        public string Pedext { get; set; }

        [XmlElement(ElementName = "pedido", Namespace = "")]
        public string Pedido { get; set; }

        [XmlElement(ElementName = "artpv1", Namespace = "")]
        public string Artpv1 { get; set; }

        [XmlElement(ElementName = "artpv2", Namespace = "")]
        public string Artpv2 { get; set; }

        [XmlElement(ElementName = "canteo", Namespace = "")]
        public string Canteo { get; set; }

        [XmlElement(ElementName = "lotefa", Namespace = "")]
        public string LoteFa { get; set; }
    }

    [XmlRoot(ElementName = "confLineaRecep", Namespace = "")]
    public class ConfLineaRecep
    {

        [XmlElement(ElementName = "ConfLineaRecepcion", Namespace = "")]
        public List<ConfLineaRecepcion> ConfLineaRecepcion { get; set; }
    }

    [XmlRoot(ElementName = "recibirConfRecepcion", Namespace = "")]
    public class RecibirConfRecepcion
    {
        [XmlElement(ElementName = "message", Namespace = "")]
        [XmlAnyElement(Namespace = "http://www.w3.org/2001/XMLSchema-instance", Name = "MessageHeaderRequest")]
        public Message Message { get; set; }

        [XmlElement(ElementName = "confRecepcion", Namespace = "")]
        public ConfRecepcion ConfRecepcion { get; set; }

        [XmlElement(ElementName = "confLineaRecep", Namespace = "")]
        public ConfLineaRecep ConfLineaRecep { get; set; }

    }
}
