using System;

namespace BusinessLogicLayer.Dto.Query
{
    public class EntradaMovimientoDevolucionClienteDetalleDto
    {
        public int id_item { get; set; }
        public string id_bodega { get; set; }
        public string id_bodega_ap { get; set; }
        public string id_bodega_np { get; set; }
        public string id_ubicacion_aux { get; set; }
        public string id_motivo { get; set; }
        public string id_co_movto { get; set; }
        public string id_un_movto { get; set; }
        public string id_unidad_medida { get; set; }
        public string rowid_movto { get; set; }
        public DateTime dato_fecha { get; set; }
        public string ind_obsequio { get; set; }
        public string ind_impto_asumido { get; set; }
        public string id_co { get; set; }
        public string id_tipo_docto { get; set; }
        public string consec_docto { get; set; }
    }
}
