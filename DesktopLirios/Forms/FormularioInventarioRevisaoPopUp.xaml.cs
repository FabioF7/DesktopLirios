using DesktopLirios.Common;
using DesktopLirios.Requests;
using DesktopLirios.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DesktopLirios
{
    public partial class FormularioInventarioRevisaoPopUp : Window
    {
        private SecureString jwtToken;
        public InventarioRequest? Inventario { get; set; } = new InventarioRequest();
        private int? idInventario;
        public List<ProdutoResponse> ListaRevisao = new List<ProdutoResponse>();

        public FormularioInventarioRevisaoPopUp(SecureString token, int? id)
        {
            InitializeComponent();
            jwtToken = token;
            idInventario = id;
            CenterWindowOnScreen();
            CarregarListaRevisao();
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            double windowWidth = Width;
            double windowHeight = Height;

            Left = (screenWidth - windowWidth) / 2;
            Top = (screenHeight - windowHeight) / 2;
        }

        private async Task CarregarListaRevisao()
        {
            try
            {
                var response = await InventarioDetalhesAPI.InventarioDetalhesApi(null, idInventario, "Get", jwtToken);

                var lista = JsonConvert.DeserializeObject<List<InventarioDetalhesResponse>>(response);

                var produtos = ProdutoGlobal.produtoGlobal;

                foreach (var item in lista)
                {
                    var prodRevisao = produtos.Where(P => P.Id == item.IdProduto).FirstOrDefault();

                    ListaRevisao.Add(prodRevisao);
                }

                ConfigureDataGridColumns();

                grdRevisao.ItemsSource = ListaRevisao;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar lista revisão: {ex.Message}");
            }
        }

        private void ConfigureDataGridColumns()
        {
            grdRevisao.AutoGenerateColumns = false;

            grdRevisao.Columns.Clear();

            DataGridTextColumn colunaNome = new DataGridTextColumn();
            colunaNome.Header = "Nome";
            colunaNome.Binding = new Binding("Nome");
            grdRevisao.Columns.Add(colunaNome);

            DataGridTextColumn colunaQuantidade = new DataGridTextColumn();
            colunaQuantidade.Header = "Previsão";
            colunaQuantidade.Binding = new Binding("Quantidade");
            grdRevisao.Columns.Add(colunaQuantidade);

            DataGridTextColumn colunaContabilizado = new DataGridTextColumn();
            colunaContabilizado.Header = "Contabilizado";
            colunaContabilizado.Binding = new Binding("Contabilizado");
            grdRevisao.Columns.Add(colunaContabilizado);

            DataGridTextColumn colunaId = new DataGridTextColumn();
            colunaId.Header = "Id";
            colunaId.Binding = new Binding("Id");
            colunaId.Visibility = Visibility.Collapsed;
            grdRevisao.Columns.Add(colunaId);

        }

        private void btnAprovar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
