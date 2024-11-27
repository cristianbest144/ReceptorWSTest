using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entites
{
    [Table("InterfaceReceptor")]
    public class InterfaceReceptor
    {
        [Key]
        public int Id { get; set; }
        public bool IsStatus { get; set; }
        public string Name { get; set; }
        public string KeyName { get; set; }
        public int CompanyId { get; set; }
        public string MessageLog { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int OrderId { get; set; }
        public DateTime CreateDatime { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
