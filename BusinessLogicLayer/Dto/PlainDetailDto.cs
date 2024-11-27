using DataAccessLayer.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Dto
{
    public class PlainDetailDto
    {
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual string KeyName { get; set; }
        [StringLength(maximumLength: 255)]
        public virtual string Description { get; set; }
        public virtual ICollection<PlainSectionDetailDto> Sections { get; set; }

    }

    public class PlainSectionDetailDto
    {
        [Required]
        public virtual string keyName { get; set; }
        [Required]
        public virtual string Name { get; set; }
        public virtual int Order { get; set; }
        public int PlainId { get; set; }

        public bool AllowMultipleRows { get; set; }

        public virtual ICollection<PlainFieldsDetailDto> Fields { get; set; }
    }

    public class PlainFieldsDetailDto
    {
        public virtual int PlainSectionId { get; set; }
        public int Order { get; set; }
        public string FieldName { get; set; }
        public PlainFieldType Type { get; set; }
        public int Size { get; set; }
        public bool IsVariable { get; set; }
        public string DefaultValue { get; set; }
        public string Description { get; set; }
        public string Observations { get; set; }
        public string Format { get; set; }
    }

    public class PagedKeywordResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
