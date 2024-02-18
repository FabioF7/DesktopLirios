using System;

namespace DesktopLirios.Requests
{
    public class VendaRequest
    {
        public int IdVenda { get; set; }
        public float ValorVenda { get; set; }
        public DateTime DtVenda { get; set; }
        public int ClienteId { get; set; }
        public int ProdutoId { get; set; }
        public float CustoProduto { get; set; }
        public int MetodoPagamento { get; set; }
        public int Tipo { get; set; }
        public int TipoTransacao { get; set; }
        public float Lucro { get; set; }
        public int Quantidade { get; set; }
        public int? PreVenda { get; set; }
        public string? CadastradoPor { get; set; }
        public string? AlteradoPor { get; set; }
        public DateTime? DtAlteracao { get; set; }
    }
}