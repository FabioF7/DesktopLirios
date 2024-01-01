using System;

namespace DesktopLirios.Responses
{
    public class OrigemResponse
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int Ativo { get; set; }
        public DateTime DtCadastro { get; set; }
        public string? CadastradoPor { get; set; }
        public DateTime DtAlteracao { get; set; }
        public string? AlteradoPor { get; set; }

    }
}
