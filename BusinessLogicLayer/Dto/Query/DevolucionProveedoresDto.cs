using System;

namespace BusinessLogicLayer.Dto.Query
{
    public class DevolucionProveedoresDto
    {
        public string id_unidad_medida { get; set; }
        public string id_bodega_AP { get; set; }
        public string id_bodega_NA { get; set; }
        public string id_un_movto { get; set; }
        public string costo_uni { get; set; }
        public int dec_compra_venta { get; set; }

    }
}
