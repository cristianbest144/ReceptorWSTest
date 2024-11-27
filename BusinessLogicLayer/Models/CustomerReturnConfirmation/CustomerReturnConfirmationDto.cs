using System.Collections.Generic;

namespace BusinessLogicLayer.Models
{
    public class CustomerReturnConfirmationDto
    {
        public int CompanyId { get; set; }
        public int SiesaCode { get; set; }
        public MessageDto Message { get; set; }
        public ConfDevolucionClienteDto ConfDevolucionCliente { get; set; }
        public ConfLineaDevolucionClienteDto ConfLineaDevolucionCliente { get; set; }
    }
    public class ConfDevolucionClienteDto
    {
        public string Sitcab { get; set; }
        public string Numdoc { get; set; }
        public string Almace { get; set; }
        public string Accion { get; set; }
        public string Cliente { get; set; }
        public string Fecha { get; set; }
        public string Cliext { get; set; }
        public string Totbul { get; set; }
        public string Codext { get; set; }
        public string Fecdev { get; set; }
        public string Codigo { get; set; }
        public string Propie { get; set; }
    }

    public class ConfLineaDevolucionClienteDto
    {
        public List<LineaDevolucionClienteDto> LineasDevolucionCliente { get; set; }
    }

    public class LineaDevolucionClienteDto
    {
        public string Varlog { get; set; }
        public string Varia1 { get; set; }
        public string Varia2 { get; set; }
        public string Sitlin { get; set; }
        public string Artpvl { get; set; }
        public string Canrea { get; set; }
        public string Fecdev { get; set; }
        public string Articu { get; set; }
        public string Artpro { get; set; }
        public string Lotefa { get; set; }
        public string Numbul { get; set; }
        public string Causad { get; set; }
        public string Cantna { get; set; }
        public string Feccad { get; set; }
        public string Codlin { get; set; }
        public string Codigo { get; set; }
        public string Accion { get; set; }
        public string Abonli { get; set; }
        public string Artpv1 { get; set; }
        public string Artpv2 { get; set; }
        public string Cantot { get; set; }
        public string Fecha { get; set; }
    }
}
