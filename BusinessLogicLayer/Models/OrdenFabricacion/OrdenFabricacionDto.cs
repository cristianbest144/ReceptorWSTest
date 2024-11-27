namespace BusinessLogicLayer.Models
{
    public class OrdenFabricacionDto
    {
        public int CompanyId { get; set; }
        public int SiesaCode { get; set; }
        public MessageDto Message { get; set; }
        public ConfOrdenFabricacionDto ConfOrdenFabricacion { get; set; }
    }
    public class ConfOrdenFabricacionDto
    {
        public string Accion { get; set; }
        public string Articu { get; set; }
        public string Artpro { get; set; }
        public string Artpv1 { get; set; }
        public string Artpv2 { get; set; }
        public string Artpvl { get; set; }
        public string Cantid { get; set; }
        public string Codgof { get; set; }

        public string Codofr { get; set; }
        public string Feccad { get; set; }
        public string Fecha { get; set; }
        public string Fecreg { get; set; }
        public string Gofext { get; set; }
        public string Lotefa { get; set; }
        public string Ofrext { get; set; }
        public string Varia1 { get; set; }
        public string Varia2 { get; set; }
        public string Varlog { get; set; }
    }
}
