using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entites
{
    [Table("CustomizationUI")]
    public class CustomizationUI
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Company")]
        public virtual long Id_Cia { get; set; }
        public virtual Company Company { get; set; }
        [Required]
        public string Logo { get; set; }
        public string Footer { get; set; }
        public string Color { get; set; }
        public string Favicon { get; set; }
        public string Name_Favicon { get; set; }
        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public bool? IsDeleted { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
