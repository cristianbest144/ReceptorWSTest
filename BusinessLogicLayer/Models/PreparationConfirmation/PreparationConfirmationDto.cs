using System.Collections.Generic;

namespace BusinessLogicLayer.Models
{
    public class PreparationConfirmationDto
    {
        public int CompanyId { get; set; }
        public int SiesaCode { get; set; }
        public MessageDto Message { get; set; }
        public ConfPreparacionDto ConfPreparacion { get; set; }
        public ConfLineaPrepDto ConfLineaPrep { get; set; }
        public ConfNSPrepDto ConfNSPrep { get; set; }
        public ConfPaletaPrepDto ConfPaletaPrep { get; set; }
    }
    public class ConfPreparacionDto
    {
        public string Almace { get; set; }
        public string Totbul { get; set; }
        public string Accion { get; set; }
        public string Fecser { get; set; }
        public string Fectra { get; set; }
        public string Fecha { get; set; }
        public string Propie { get; set; }
        public string Pedido { get; set; }
        public string Sitped { get; set; }
        public string Client { get; set; }
        public string Tipped { get; set; }
        public string Pedext { get; set; }
        public string Usuari { get; set; }
        public string Agenci { get; set; }
        public string Divped { get; set; }
        public string Cliext { get; set; }
    }

    public class ConfLineaPrepDto
    {
        public List<LineaPreparacionDto> LineasPreparacion { get; set; }
    }

    public class LineaPreparacionDto
    {
        public string Varlog { get; set; }
        public string Varia1 { get; set; }
        public string Varia2 { get; set; }
        public string Sitlin { get; set; }
        public string Causaf { get; set; }
        public string Artpvl { get; set; }
        public string Bulrec { get; set; }
        public string Fecha { get; set; }
        public string Articu { get; set; }
        public string Lote { get; set; }
        public string Canpes { get; set; }
        public string Feccad { get; set; }
        public string Canrec { get; set; }
        public string Codlin { get; set; }
        public string Accion { get; set; }
        public string Pedext { get; set; }
        public string Pedido { get; set; }
        public string Artpro { get; set; }
        public string Artpv1 { get; set; }
        public string Divped { get; set; }
        public string Artpv2 { get; set; }
        public string Canped { get; set; }
        public string Almkit { get; set; }
        public string Pprkit { get; set; }
    }

    public class ConfNSPrepDto
    {
        // Aquí puedes agregar las propiedades necesarias para representar la sección <confNSPrep>, si es que existe.
    }

    public class ConfPaletaPrepDto
    {
        // Aquí puedes agregar las propiedades necesarias para representar la sección <confPaletaPrep>, si es que existe.
    }

}
