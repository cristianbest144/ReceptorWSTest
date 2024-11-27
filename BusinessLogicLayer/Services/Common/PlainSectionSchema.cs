using DataAccessLayer.Entites;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services.Common
{
    public class PlainSectionSchema
    {
        public PlainSectionSchema(int id, int order, string name, string keyName, ICollection<PlainField> fields, bool isRequired)
        {
            Id = id;
            Order = order;
            KeyName = keyName.Trim();
            Fields = fields.OrderBy(x => x.Order)
                                .ToDictionary(x => x.FieldName.Trim(), x => new PlainFieldSchema
                                {
                                    DefaultValue = x.DefaultValue,
                                    Type = x.Type,
                                    Size = x.Size,
                                    Order = x.Order,
                                    IsVariable = x.IsVariable,
                                    FieldName = x.FieldName.Trim(),
                                    Format = x.Format
                                });
            IsRequired = isRequired;
        }
        public int Order { get; set; }
        public string KeyName { get; set; }
        public Dictionary<string, PlainFieldSchema> Fields { get; }
        public int Id { get; set; }
        public bool IsRequired { get; set; }
    }
}
