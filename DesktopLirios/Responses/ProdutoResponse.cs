using System;

namespace DesktopLirios.Responses
{
    public class ProdutoResponse
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int OrigemId { get; set; }
        public string? Codigo { get; set; }
        public int CodigoDeBarra { get; set; }
        public float ValorCusto { get; set; }
        public float ValorVendaRevista { get; set; }
        public int IdCategoria { get; set; } //Verificar se irá criar Enum
        public int Quantidade { get; set; }
        public int Ativo { get; set; }
        public string? CadastradoPor { get; set; }
        public DateTime DtCadastro { get; set; }
        public string? AlteradoPor { get; set; }
        public DateTime DtAlteracao { get; set; }

    }
}
