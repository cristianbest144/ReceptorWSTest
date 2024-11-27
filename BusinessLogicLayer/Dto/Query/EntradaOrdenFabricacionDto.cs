namespace BusinessLogicLayer.Dto.Query
{
    public class EntradaOrdenFabricacionDto
    {
        public string rowid { get; set; }
        public string id_item { get; set; }
        public string id_item_padre { get; set; }
        public float cant_base { get; set; }
        public string id_bodega_salida { get; set; }
        public string id_bodega_entrada { get; set; }
        public string id_unidad_medida { get; set; }
        public string id_un_movto { get; set; }
    }
}
