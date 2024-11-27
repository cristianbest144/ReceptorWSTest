using DataAccessLayer.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Dto
{
    public class ConfigurationDto
    {
       public string ws_siesa_conexion { get; set; }
       public string user_siesa { get; set; }
       public string pass_siesa { get; set; }
       public string url { get; set; }
       public string action { get; set; }

    }
}
