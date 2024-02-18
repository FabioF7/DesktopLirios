using System;
using System.ComponentModel;

namespace DesktopLirios.Responses
{
    public class ClienteResponse : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int? _id;
        public int? Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private string? _nome;
        public string? Nome
        {
            get { return _nome; }
            set
            {
                _nome = value;
                OnPropertyChanged(nameof(Nome));
            }
        }

        private string? _email;
        public string? Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private long _celular;
        public long Celular
        {
            get { return _celular; }
            set
            {
                _celular = value;
                OnPropertyChanged(nameof(Celular));
            }
        }

        private string? _cep;
        public string? CEP
        {
            get { return _cep; }
            set
            {
                _cep = value;
                OnPropertyChanged(nameof(CEP));
            }
        }

        private string? _endereco;
        public string? Endereco
        {
            get { return _endereco; }
            set
            {
                _endereco = value;
                OnPropertyChanged(nameof(Endereco));
            }
        }

        private DateTime _dtNascimento;
        public DateTime DtNascimento
        {
            get { return _dtNascimento; }
            set
            {
                _dtNascimento = value;
                OnPropertyChanged(nameof(DtNascimento));
            }
        }

        private int _sexo;
        public int Sexo
        {
            get { return _sexo; }
            set
            {
                _sexo = value;
                OnPropertyChanged(nameof(Sexo));
            }
        }

        private int? _indicacao;
        public int? Indicacao
        {
            get { return _indicacao; }
            set
            {
                _indicacao = value;
                OnPropertyChanged(nameof(Indicacao));
            }
        }

        private int _bloqueado;
        public int Bloqueado
        {
            get { return _bloqueado; }
            set
            {
                _bloqueado = value;
                OnPropertyChanged(nameof(Bloqueado));
            }
        }

        private int _inadimplencia;
        public int Inadimplencia
        {
            get { return _inadimplencia; }
            set
            {
                _inadimplencia = value;
                OnPropertyChanged(nameof(Inadimplencia));
            }
        }

        private float? _limiteInadimplencia;
        public float? LimiteInadimplencia
        {
            get { return _limiteInadimplencia; }
            set
            {
                _limiteInadimplencia = value;
                OnPropertyChanged(nameof(LimiteInadimplencia));
            }
        }

        private string? _observacoes;
        public string? Observacoes
        {
            get { return _observacoes; }
            set
            {
                _observacoes = value;
                OnPropertyChanged(nameof(Observacoes));
            }
        }

        private string? _cadastradoPor;
        public string? CadastradoPor
        {
            get { return _cadastradoPor; }
            set
            {
                _cadastradoPor = value;
                OnPropertyChanged(nameof(CadastradoPor));
            }
        }

        private DateTime? _dtCadastro;
        public DateTime? DtCadastro
        {
            get { return _dtCadastro; }
            set
            {
                _dtCadastro = value;
                OnPropertyChanged(nameof(DtCadastro));
            }
        }

        private string? _alteradoPor;
        public string? AlteradoPor
        {
            get { return _alteradoPor; }
            set
            {
                _alteradoPor = value;
                OnPropertyChanged(nameof(AlteradoPor));
            }
        }

        private DateTime? _dtAlteracao;
        public DateTime? DtAlteracao
        {
            get { return _dtAlteracao; }
            set
            {
                _dtAlteracao = value;
                OnPropertyChanged(nameof(DtAlteracao));
            }
        }

        private string? _devido;
        public string? Devido
        {
            get { return _devido; }
            set
            {
                _devido = value;
                OnPropertyChanged(nameof(Devido));
            }
        }

        private string? _limiteLivre;
        public string? LimiteLivre
        {
            get { return _limiteLivre; }
            set
            {
                _limiteLivre = value;
                OnPropertyChanged(nameof(LimiteLivre));
            }
        }

        // Método auxiliar para invocar o evento PropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
