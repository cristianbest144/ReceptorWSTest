using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entites
{
    [Table("ProcessConfOrdenFabricacion")]
    public class ProcessConfOrdenFabricacion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public  int CompanyId { get; set; }
        [Required]
        public int SiesaCode { get; set; }
        public string GofExt { get; set; }
        public string OfrExt { get; set; }
        public string Articu { get; set; }
        public string ArtPro { get; set; }
        public float Cantid { get; set; }
        public string LoteFa { get; set; }
        public string FecCad { get; set; }
        public string FecReg { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Source { get; set; }
    }
}
