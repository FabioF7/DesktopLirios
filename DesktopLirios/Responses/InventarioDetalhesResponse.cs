using System;

namespace DesktopLirios.Responses
{
    public class InventarioDetalhesResponse
    {
        public int? Id { get; set; }

        public int? IdInventario { get; set; }

        public int? IdProduto { get; set; }

        public int? Previsao { get; set; }

        public int? Contabilizado { get; set; }
    }
}
