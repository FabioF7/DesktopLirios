using System;

namespace DesktopLirios.Requests
{
    public class InventarioDetalhesRequest
    {
        public int? IdInventario { get; set; }
        public int? IdProduto { get; set; }
        public int? Previsao { get; set; }
        public int? Contabilizado { get; set; }
    }
}