using System;

namespace BusinessLogicLayer.Dto.Query
{
    public class EntradaAlmacenCabeceraDetalleDto
    {
        public string id_item { get; set; }
        public string id_bodega { get; set; }
        public string id_ubicacion_aux { get; set; }
        public string id_unidad_medida { get; set; }
        public DateTime fecha_entrega { get; set; }
        public int factor { get; set; }
        public string rowid { get; set; }
        public string rowidright { get; set; }
        public string id_unidad_inventario { get; set; }
}
}
