using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entites
{
    [Table("InputParameter")]
    public class InputParameter
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Company")]
        public long CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [ForeignKey("ParameterOption")]
        public int ParameterOptionId { get; set; }
        public virtual ParameterOption ParameterOption { get; set; }

        public string SupplierItemPlan { get; set; }
        public string CategoryItemPlan { get; set; }
        public string SubcategoryItemPlan { get; set; }
        public string AppliedMarginItem { get; set; }
        public string CostOverrunItem { get; set; }
        public string DocumentType { get; set; }
        public int? Consecutive { get; set; }
        public string PlainKeyName { get; set; }
        public DateTime? LastGenerationDate { get; set; }
        //public string FilterAdditionals { get; set; }

        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public bool? IsDeleted { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public string CO { get; set; }

    }
}
