using DataAccessLayer.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entites
{
    [Table("ProcessDataStock")]
    public class ProcessDataStock
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public  string PlainKeyName { get; set; }
        [Required]
        public string KeyName { get; set; }
        public string SiesaCode { get; set; }
        public string f470_id_co { get; set; }
        public string f470_id_bodega { get; set; }
        public string f470_id_co_movto { get; set; }
        public string f470_id_unidad_medida { get; set; }
        public string f470_cant_base { get; set; }
        public string f470_id_item { get; set; }
        public string KeyHeader { get; set; }
        public string KeyItems { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? LastProcessDate { get; set; }
        public string f450_id_bodega_salida { get; set; }
        public string f450_id_bodega_entrada { get; set; }
        public string Source { get; set; }
        public int CompanyId { get; set; }
    }
}
