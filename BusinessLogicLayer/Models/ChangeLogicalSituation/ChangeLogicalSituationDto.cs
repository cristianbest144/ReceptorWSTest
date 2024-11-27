namespace BusinessLogicLayer.Models
{
    public class ChangeLogicalSituationDto
    {
        public int CompanyId { get; set; }
        public int SiesaCode { get; set; }
        public MessageDto Message { get; set; }
        public ConfCambioSituacionLogicaDto ConfCambioSituacionLogica { get; set; }
    }
    public class ConfCambioSituacionLogicaDto
    {
        public string Stviej { get; set; }
        public string Stnuev { get; set; }
        public string Almace { get; set; }
        public string Accion { get; set; }
        public string Cantid { get; set; }
        public string Coment { get; set; }
        public string Varlog { get; set; }
        public string Varia1 { get; set; }
        public string Varia2 { get; set; }
        public string Feccad { get; set; }
        public string Lotefa { get; set; }
        public string Feccam { get; set; }
        public string Propie { get; set; }
        public string Articu { get; set; }
        public string Artpro { get; set; }
        public string Artpv1 { get; set; }
        public string Artpv2 { get; set; }
        public string Artpvl { get; set; }
        public string Causal { get; set; }
        public string Codaju { get; set; }
    }

}
