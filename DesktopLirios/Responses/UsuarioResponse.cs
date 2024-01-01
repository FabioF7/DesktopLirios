using System;

namespace DesktopLirios.Responses
{
    public class UsuarioResponse
    {
        public int UserID { get; set; }
        public string? Nome { get; set; }
        public string? Usuario { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]?  PasswordSalt { get; set; }
        public DateTime DtCadastro { get; set; }
        public DateTime DtAlteracao { get; set; }
        public int Ativo { get; set; }
        public int IdPerfil { get; set; }
    }
}
