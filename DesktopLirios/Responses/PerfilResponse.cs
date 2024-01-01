using System;

namespace DesktopLirios.Responses
{
    public class PerfilResponse
    {
        public int Id { get; set; }
        public string? NomePerfil { get; set; }
        public string? DescricaoPerfil { get; set; }
        public int Ativo { get; set; }
        public DateTime DtCadastro { get; set; }
        public DateTime DtAlteracao { get; set; }
    }
}
