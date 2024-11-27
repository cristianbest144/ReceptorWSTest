using System.Collections.Generic;

namespace BusinessLogicLayer.Models
{
    public class RecibirConfCargaCamionDto
    {
        public int CompanyId { get; set; }
        public int SiesaCode { get; set; }
        public MessageDto Message { get; set; }
        public ConfCargaCamionDto ConfCargaCamion { get; set; }
        public List<ConfLineaCargaCamionDto> ConfLineaCargaCamion { get; set; }
    }
    public class ConfCargaCamionDto
    {
        public string Codcot { get; set; }
        public string Feccar { get; set; }
        public string Matcot { get; set; }
        public string Centro { get; set; }
        public string Vpl { get; set; }
        public string Estcot { get; set; }
        public string Accion { get; set; }
        public string Pesmax { get; set; }
        public string Volume { get; set; }
        public string Fecha { get; set; }
    }

    public class ConfLineaCargaCamionDto
    {
        public string Codcot { get; set; }
        public string Pedido { get; set; }
        public string Pedext { get; set; }
        public string Orden { get; set; }
        public string Accion { get; set; }
        public string Pesped { get; set; }
        public string Cajaco { get; set; }
        public string Cajare { get; set; }
        public string Bolsas { get; set; }
        public string Canast { get; set; }
        public string Listct { get; set; }
        public string Fecha { get; set; }

    }

}
