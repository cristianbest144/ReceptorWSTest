using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    public class EscribirPlanoOutput
    {
        public List<string> Errors { get; set; }
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
    }

    public class SectionData
    {
        public string KeyName { get; set; }

        public ICollection<FieldData> Fields { get; set; }

        public Dictionary<string, string> FieldsDictionary { get { return Fields.ToDictionary(x => x.KeyName, x => x.Value); } }
    }

    public class FieldData
    {
        public string KeyName { get; set; }
        public string Value { get; set; }
    }

    public class PlainDataInput
    {
        [Required]
        public int Company { get; set; }
        [Required]
        public string KeyName { get; set; }
        public List<SectionData> Sections { get; set; }
    }


}
