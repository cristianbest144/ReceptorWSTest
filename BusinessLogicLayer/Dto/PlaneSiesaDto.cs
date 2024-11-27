using System.Collections.Generic;

namespace BusinessLogicLayer.Dto
{
    public class PlaneSiesaDto
    {
        public string NombreConexion { get; set; }
        public int IdCia { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string Datos { get; set; }
        public virtual List<string> Linea { get; set; }
    }
}
