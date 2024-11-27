using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entites
{
    [Table("PlainSection")]
    public class PlainSection
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public virtual string KeyName { get; set; }
        [Required]
        public virtual string Name { get; set; }

        public virtual int Order { get; set; }

        public virtual bool AllowMultipleRows { get; set; }

        [ForeignKey("Plain")]
        public int PlainId { get; set; }
        public virtual Plain Plain { get; set; }
        public virtual ICollection<PlainField> Fields { get; set; }
        public bool IsRequired { get; set; }

        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public bool? IsDeleted { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
