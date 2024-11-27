using System.Collections.Generic;

namespace BusinessLogicLayer.Models
{
    public class RecibirConfRecepcionDto
    {
        public int CompanyId { get; set; }
        public int SiesaCode { get; set; }
        public MessageDto Message { get; set; }
        public ConfRecepcionDto ConfRecepcion { get; set; }
        public ConfLineaRecepDto ConfLineaRecep { get; set; }
        public string ConfNSRecep { get; set; }
        public string ConfPaletaRecep { get; set; }
    }


    public class ConfRecepcionDto
    {
        public string Sitcab { get; set; }
        public string Nument { get; set; }
        public string Almace { get; set; }
        public string Numpal { get; set; }
        public string Fecha { get; set; }
        public string Pedido { get; set; }
        public string Usuari { get; set; }
        public string Proext { get; set; }
        public string Fecfin { get; set; }
        public string Provee { get; set; }
        public string Tipped { get; set; }
        public string Propie { get; set; }
        public string Pedext { get; set; }
        public string Albara { get; set; }
    }

    public class ConfLineaRecepDto
    {
        public List<ConfLineaRecepcionDto> ConfLineaRecepcion { get; set; }
    }

    public class ConfLineaRecepcionDto
    {
        public string Varlog { get; set; }
        public string Varia1 { get; set; }
        public string Varia2 { get; set; }
        public string Sitlin { get; set; }
        public string Artpvl { get; set; }
        public string Bulrec { get; set; }
        public string Fecha { get; set; }
        public string Articu { get; set; }
        public string Artpro { get; set; }
        public string Nument { get; set; }
        public string Cantco { get; set; }
        public string Canpes { get; set; }
        public string Feccad { get; set; }
        public string Codlin { get; set; }
        public string Accion { get; set; }
        public string Pedext { get; set; }
        public string Pedido { get; set; }
        public string Artpv1 { get; set; }
        public string Artpv2 { get; set; }
        public string Canteo { get; set; }
        public string LoteFa { get; set; }
    }
}
