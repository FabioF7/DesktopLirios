using System;

namespace DesktopLirios.Requests
{
    public class InventarioRequest
    {
        public string? Nome { get; set; }
        public string? Situacao { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Concluido { get; set; }
        public string? Contado_por { get; set; }
        public string? Revisado_por { get; set; }
        public string? Obsevacoes { get; set; }
        public int? Contabilizado_Total { get; set; }
    }
}