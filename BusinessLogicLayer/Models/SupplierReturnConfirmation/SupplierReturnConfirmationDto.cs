using System.Collections.Generic;

namespace BusinessLogicLayer.Models
{
    public class SupplierReturnConfirmationDto
    {
        public int CompanyId { get; set; }
        public int SiesaCode { get; set; }
        public MessageDto Message { get; set; }
        public ConfDevolucionProveedorDto ConfDevolucionProveedor { get; set; }
        public ConfLineaDevolucionProveedorDto ConfLineaDevolucionProveedor { get; set; }
        public ConfNSDevolucionProveedorDto ConfNSDevolucionProveedor { get; set; }
    }

    public class ConfDevolucionProveedorDto
    {
        public string Sitcab { get; set; }
        public string Numdoc { get; set; }
        public string Almace { get; set; }
        public string Accion { get; set; }
        public string Proext { get; set; }
        public string Fecha { get; set; }
        public string Provee { get; set; }
        public string Fecdev { get; set; }
        public string Codext { get; set; }
        public string Codigo { get; set; }
        public string Propie { get; set; }
    }

    public class ConfLineaDevolucionProveedorDto
    {
        public List<LineaDevolucionProveedorDto> LineasDevolucionProveedor { get; set; }
    }

    public class LineaDevolucionProveedorDto
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
        public string Causad { get; set; }
        public string Cantid { get; set; }
        public string Feccad { get; set; }
        public string Codlin { get; set; }
        public string Codext { get; set; }
        public string Codigo { get; set; }
        public string Accion { get; set; }
        public string Aptnap { get; set; }
        public string Artpv1 { get; set; }
        public string Artpv2 { get; set; }
        public string Fecha { get; set; }
        public string LoteFa { get; set; }
    }

    public class ConfNSDevolucionProveedorDto
    {
        // Aquí puedes agregar las propiedades necesarias para representar la sección <confNSDevolucionProveedor>, si es que existe.
    }

}
