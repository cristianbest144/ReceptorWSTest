using DataAccessLayer.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entites
{
    [Table("PlainField")]
    public class PlainField
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("PlainSection")]
        public virtual int PlainSectionId { get; set; }
        public virtual PlainSection PlainSection { get; set; }
        public int Order { get; set; }
        public string FieldName { get; set; }
        public PlainFieldType Type { get; set; }
        public int Size { get; set; }
        public bool IsVariable { get; set; }
        public string DefaultValue { get; set; }
        public string Description { get; set; }
        public string Observations { get; set; }
        public string Format { get; set; }
        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public bool? IsDeleted { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
