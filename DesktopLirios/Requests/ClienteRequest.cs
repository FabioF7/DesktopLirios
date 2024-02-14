using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DesktopLirios.Requests
{
        public class ClienteRequest
        {
            public string? Nome { get; set; }
            public string? Email { get; set; }
            public Int64? Celular { get; set; }
            public string? CEP { get; set; }
            public string? Endereco { get; set; }
            public DateTime? DtNascimento { get; set; }
            public int? Sexo { get; set; }
            public int? Indicacao { get; set; }
            public int? Bloqueado { get; set; }
            public int Inadimplencia { get; set; }
            public float? LimiteInadimplencia { get; set; }
            public string? Observacoes { get; set; }
            public string? AlteradoPor { get; set; }
            public DateTime? DtAlteracao { get; set; }
            public string? CadastradoPor { get; set; }
            public DateTime? DtCadastro { get; set; }
    }
}