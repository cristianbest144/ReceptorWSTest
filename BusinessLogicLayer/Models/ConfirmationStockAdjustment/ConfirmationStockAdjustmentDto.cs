namespace BusinessLogicLayer.Models
{

    public class ConfirmationStockAdjustmentDto
    {
        public int CompanyId { get; set; }
        public int SiesaCode { get; set; }
        public MessageDto Message { get; set; }
        public ConfRegularizacionStockDto ConfRegularizacionStock { get; set; }
    }
    public class ConfRegularizacionStockDto
    {
        public string Varlog { get; set; }
        public string Fecaju { get; set; }
        public string Varia1 { get; set; }
        public string Varia2 { get; set; }
        public string Propie { get; set; }
        public string Causal { get; set; }
        public string Artpvl { get; set; }
        public string Cantid { get; set; }
        public string Fecha { get; set; }
        public string Articu { get; set; }
        public string Artpro { get; set; }
        public string Bulrec { get; set; }
        public string Signoa { get; set; }
        public string Paleta { get; set; }
        public string Feccad { get; set; }
        public string Codaju { get; set; }
        public string Almace { get; set; }
        public string Accion { get; set; }
        public string Artpv1 { get; set; }
        public string Artpv2 { get; set; }
        public string Codmov { get; set; }
        public string Sitlog { get; set; }
        public string Lotefa { get; set; }
    }
}
