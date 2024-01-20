using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DesktopLirios.Requests
{
    public class ProdutoRequest
    {
        public string? Nome { get; set; }
        public int OrigemId { get; set; }
        public string? Codigo { get; set; }
        public Int64? CodigoDeBarra { get; set; }
        public float ValorCusto { get; set; }
        public float ValorVendaRevista { get; set; }
        public int? IdCategoria { get; set; }
        public int Quantidade { get; set; }
        public int Ativo { get; set; }
        public string? CadastradoPor { get; set; }
        public DateTime DtCadastro { get; set; }
        public string? AlteradoPor { get; set; }
        public DateTime DtAlteracao { get; set; }
    }
}