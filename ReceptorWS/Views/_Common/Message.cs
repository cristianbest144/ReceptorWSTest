using System.Xml.Serialization;

namespace PresentationLayer.Views
{
    //public class MessageHeaderRequest
    //{
    //    [XmlElement(ElementName = "messageInfo")]
    //    public MessageInfo MessageInfo { get; set; }

    //    public string trxId { get; set; }
    //    public string correlationId { get; set; }
    //}
    [XmlRoot("message")]
    [XmlType(TypeName = "MessageHeaderRequest")]
    public class Message
    {
        [XmlElement(ElementName = "messageInfo", Namespace = "")]
        public MessageInfo MessageInfo { get; set; }
        [XmlElement(ElementName = "trxId", Namespace = "")]
        public string trxId { get; set; }
    }

    public class MessageInfo
    {
        [XmlElement(ElementName = "dateTime", Namespace = "")]
        public string dateTime { get; set; }
        [XmlElement(ElementName = "originatorName", Namespace = "")]
        public string originatorName { get; set; }
    }

}