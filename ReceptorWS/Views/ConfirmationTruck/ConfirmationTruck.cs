namespace PresentationLayer.Views
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "recibirConfCargaCamion")]
    public class recibirConfCargaCamion
    {
        [XmlElement(ElementName = "message")]
        public Message Message { get; set; }
        [XmlElement(ElementName = "confCargaCamion")]
        public ConfCargaCamion ConfCargaCamion { get; set; }

        [XmlElement(ElementName = "confLineaCargaCamion")]
        public confLineaCargaCamion confLineaCargaCamion { get; set; }
    }
    [XmlRoot(ElementName = "confCargaCamion")]
    [XmlType(TypeName = "ConfCargaCamion")]
    public class ConfCargaCamion
    {
        [XmlElement(ElementName = "codcot")]
        public string Codcot { get; set; }
        [XmlElement(ElementName = "feccar")]
        public string Feccar { get; set; }
        [XmlElement(ElementName = "matcot")]
        public string Matcot { get; set; }
        [XmlElement(ElementName = "centro")]
        public string Centro { get; set; }
        [XmlElement(ElementName = "vpl")]
        public string Vpl { get; set; }
        [XmlElement(ElementName = "estcot")]
        public string Estcot { get; set; }
        [XmlElement(ElementName = "accion")]
        public string Accion { get; set; }
        [XmlElement(ElementName = "pesmax")]
        public string Pesmax { get; set; }
        [XmlElement(ElementName = "volume")]
        public string Volume { get; set; }
        [XmlElement(ElementName = "fecha")]
        public string Fecha { get; set; }
    }


    [XmlRoot(ElementName = "confLineaCargaCamion")]
    public class confLineaCargaCamion
    {
        [XmlElement(ElementName = "ConfLineaCargaCamion", Namespace = "")]
        public List<ConfLineaCargaCamion> ConfLineaCargaCamion { get; set; }
    }
    public class ConfLineaCargaCamion
    {
        [XmlElement(ElementName = "codcot")]
        public string Codcot { get; set; }
        [XmlElement(ElementName = "pedido")]
        public string Pedido { get; set; }
        [XmlElement(ElementName = "pedext")]
        public string Pedext { get; set; }
        [XmlElement(ElementName = "orden")]
        public string Orden { get; set; }
        [XmlElement(ElementName = "accion")]
        public string Accion { get; set; }
        [XmlElement(ElementName = "pesped")]
        public string Pesped { get; set; }
        [XmlElement(ElementName = "cajaco")]
        public string Cajaco { get; set; }
        [XmlElement(ElementName = "cajare")]
        public string Cajare { get; set; }
        [XmlElement(ElementName = "bolsas")]
        public string Bolsas { get; set; }
        [XmlElement(ElementName = "canast")]
        public string Canast { get; set; }
        [XmlElement(ElementName = "listct")]
        public string Listct { get; set; }
        [XmlElement(ElementName = "fecha")]
        public string Fecha { get; set; }
    }

}
