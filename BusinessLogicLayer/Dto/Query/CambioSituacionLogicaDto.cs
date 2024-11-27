using System;

namespace BusinessLogicLayer.Dto.Query
{
    public class CambioSituacionLogicaDto
    {
        public string id_unidad_medida { get; set; }
        public string id_bodega_salida { get; set; }
        public string id_bodega_entrada { get; set; }
    }
}
