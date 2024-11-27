namespace BusinessLogicLayer.Models
{

    public class MessageDto
    {
        public MessageInfoDto MessageInfo { get; set; }
        public string TrxId { get; set; }
    }

    public class MessageInfoDto
    {
        public string DateTime { get; set; }
        public string OriginatorName { get; set; }
    }
}
