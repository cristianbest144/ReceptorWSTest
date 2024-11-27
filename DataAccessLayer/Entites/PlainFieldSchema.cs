using DataAccessLayer.Common;

namespace DataAccessLayer.Entites
{
    public class PlainFieldSchema
    {
        public string DefaultValue { get; set; }
        public PlainFieldType Type { get; set; }
        public int Size { get; set; }
        public int Order { get; set; }
        public bool IsVariable { get; set; }
        public string FieldName { get; set; }
        public string Format { get; set; }
    }
}
