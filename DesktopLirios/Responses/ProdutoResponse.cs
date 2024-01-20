using System;
using System.ComponentModel;

namespace DesktopLirios.Responses
{
    public class ProdutoResponse : INotifyPropertyChanged
    {
        private float valorVendaRevista;

        public int Id { get; set; }
        public string? Nome { get; set; }
        public int OrigemId { get; set; }
        public string? Codigo { get; set; }
        public Int64? CodigoDeBarra { get; set; }
        public float ValorCusto { get; set; }

        public float ValorVendaRevista
        {
            get { return valorVendaRevista; }
            set
            {
                if (valorVendaRevista != value)
                {
                    valorVendaRevista = value;
                    OnPropertyChanged(nameof(ValorVendaRevista));
                }
            }
        }

        public int? IdCategoria { get; set; } //Verificar se irá criar Enum
        public int Quantidade { get; set; }
        public int Ativo { get; set; }
        public string? CadastradoPor { get; set; }
        public DateTime? DtCadastro { get; set; }
        public string? AlteradoPor { get; set; }
        public DateTime? DtAlteracao { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
