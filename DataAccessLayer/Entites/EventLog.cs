using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entites
{
    [Table("EventLog")]
    public class EventLog
    {
        [Key]
        public int Id { get; set; }
        public string EventCode { get; set; }
        public string Message { get; set; }
        public string Payload { get; set; }
        public string TrackingId { get; set; }
        public bool? PayloadIsModel { get; set; }
        public string LogIndexType { get; set; }
        public string IndexType { get; set; }
        public string CallStack { get; set; }
        public string Request { get; set; }
        public DateTime Date { get; set; }
        public int CompanyId { get; set; }
    }
}
