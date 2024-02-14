using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DesktopLirios.Common;
using DesktopLirios.Responses;
using Newtonsoft.Json;

namespace DesktopLirios
{
    public partial class PaginaVendas : Page
    {
        private SecureString jwtToken;
        private VendaResponse? Venda;

        public PaginaVendas(SecureString token)
        {
            InitializeComponent();
            jwtToken = token;
            CarregarVendasAsync();
        }

        private async Task CarregarVendasAsync()
        {
            try
            {
                var response = await VendaAPI.VendaApi(null, null, "Get", null, jwtToken);

                VendaGlobal.vendaGlobal = JsonConvert.DeserializeObject<List<VendaResponse>>(response);

                ConfigureDataGridColumns();

                grdVendas.ItemsSource = VendaGlobal.vendaGlobal;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
            }
        }

        private void ConfigureDataGridColumns()
        {
            grdVendas.AutoGenerateColumns = false;

            grdVendas.Columns.Clear();

            DataGridTextColumn colunaVenda = new DataGridTextColumn();
            colunaVenda.Header = "Id Venda";
            colunaVenda.Binding = new Binding("IdVenda");
            grdVendas.Columns.Add(colunaVenda);

            DataGridTextColumn colunaCliente = new DataGridTextColumn();
            colunaCliente.Header = "Cliente";
            colunaCliente.Binding = new Binding("Cliente.Nome");
            grdVendas.Columns.Add(colunaCliente);

            DataGridTextColumn colunaProduto = new DataGridTextColumn();
            colunaProduto.Header = "Produto";
            colunaProduto.Binding = new Binding("Produto.Nome");
            grdVendas.Columns.Add(colunaProduto);

            DataGridTextColumn colunaValorVenda = new DataGridTextColumn();
            colunaValorVenda.Header = "Valor da Venda";
            colunaValorVenda.Binding = new Binding("ValorVenda") { StringFormat = "{0:0.00}", ConverterCulture = new CultureInfo("pt-BR") };
            grdVendas.Columns.Add(colunaValorVenda);

            DataGridTextColumn colunaCusto = new DataGridTextColumn();
            colunaCusto.Header = "Custo Produto";
            colunaCusto.Binding = new Binding("CustoProduto") { StringFormat = "{0:0.00}", ConverterCulture = new CultureInfo("pt-BR") };
            grdVendas.Columns.Add(colunaCusto);

            //DataGridTextColumn colunaLucro = new DataGridTextColumn();
            //colunaLucro.Header = "Lucro";
            //colunaLucro.Binding = new Binding("Lucro") { StringFormat = "{0:0.00}", ConverterCulture = new CultureInfo("pt-BR") };
            //grdVendas.Columns.Add(colunaLucro);

            DataGridTextColumn colunaQuantidade = new DataGridTextColumn();
            colunaQuantidade.Header = "Quantidade";
            colunaQuantidade.Binding = new Binding("Quantidade");
            grdVendas.Columns.Add(colunaQuantidade);

            DataGridTextColumn colunaData = new DataGridTextColumn();
            colunaData.Header = "Data Venda";
            colunaData.Binding = new Binding("DtVenda") { StringFormat = "dd/MM/yyyy" };
            grdVendas.Columns.Add(colunaData);

            DataGridTextColumn colunaMetodo = new DataGridTextColumn();
            colunaMetodo.Header = "Método de Pagamento";
            colunaMetodo.Binding = new Binding("MetodoPagamento")
            {
                Converter = new MetodoPagamentoConverter()
            };
            grdVendas.Columns.Add(colunaMetodo);

            DataGridTextColumn colunaTipo = new DataGridTextColumn();
            colunaTipo.Header = "Tipo";
            colunaTipo.Binding = new Binding("Tipo");
            grdVendas.Columns.Add(colunaTipo);

            DataGridTextColumn colunaTransacao = new DataGridTextColumn();
            colunaTransacao.Header = "Tipo Transação";
            colunaTransacao.Binding = new Binding("TipoTransacao");
            grdVendas.Columns.Add(colunaTransacao);

            DataGridTextColumn colunaPre = new DataGridTextColumn();
            colunaPre.Header = "Pré Venda";
            colunaPre.Binding = new Binding("PreVenda");
            grdVendas.Columns.Add(colunaPre);
        }

        private void txtPesquisar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string termoPesquisa = txtPesquisar.Text.ToLower();

            List<VendaResponse> VendasFiltrados = VendaGlobal.vendaGlobal
            .Where(Venda =>
                Venda.ClienteId.ToString().Contains(termoPesquisa) ||
                Venda.ProdutoId.ToString().Contains(termoPesquisa))
            .ToList();

            grdVendas.ItemsSource = VendasFiltrados;
        }
        
        private async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            await CarregarVendasAsync();
        }

        private async void btnEditar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Venda = new VendaResponse()
                {
                    IdVenda = ((VendaResponse)grdVendas.SelectedItem).IdVenda,
                    ValorVenda = ((VendaResponse)grdVendas.SelectedItem).ValorVenda,
                    DtVenda = ((VendaResponse)grdVendas.SelectedItem).DtVenda,
                    ClienteId = ((VendaResponse)grdVendas.SelectedItem).ClienteId,
                    ProdutoId = ((VendaResponse)grdVendas.SelectedItem).ProdutoId,
                    CustoProduto = ((VendaResponse)grdVendas.SelectedItem).CustoProduto,
                    MetodoPagamento = ((VendaResponse)grdVendas.SelectedItem).MetodoPagamento,
                    Tipo = ((VendaResponse)grdVendas.SelectedItem).Tipo,
                    TipoTransacao = ((VendaResponse)grdVendas.SelectedItem).TipoTransacao,
                    Quantidade = ((VendaResponse)grdVendas.SelectedItem).Quantidade,
                    PreVenda = ((VendaResponse)grdVendas.SelectedItem).PreVenda
                };

                var formularioPopup = new FormularioVendasPopup(Venda, jwtToken, "Editar");
                formularioPopup.ShowDialog();

                await CarregarVendasAsync();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Nenhum Venda selecionado!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar formulário do Venda: {ex.Message}");
            }
        }

        private async void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var formularioPopup = new FormularioVendasPopup(null, jwtToken, "Cadastrar");
                formularioPopup.ShowDialog();

                await CarregarVendasAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar formulário de cadastro de Vendas: {ex.Message}");
            }
        }

        private void btnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Venda = new VendaResponse()
                {
                    IdVenda = ((VendaResponse)grdVendas.SelectedItem).IdVenda,
                    ValorVenda = ((VendaResponse)grdVendas.SelectedItem).ValorVenda,
                    DtVenda = ((VendaResponse)grdVendas.SelectedItem).DtVenda,
                    ClienteId = ((VendaResponse)grdVendas.SelectedItem).ClienteId,
                    ProdutoId = ((VendaResponse)grdVendas.SelectedItem).ProdutoId,
                    CustoProduto = ((VendaResponse)grdVendas.SelectedItem).CustoProduto,
                    MetodoPagamento = ((VendaResponse)grdVendas.SelectedItem).MetodoPagamento,
                    Tipo = ((VendaResponse)grdVendas.SelectedItem).Tipo,
                    TipoTransacao = ((VendaResponse)grdVendas.SelectedItem).TipoTransacao,
                    Quantidade = ((VendaResponse)grdVendas.SelectedItem).Quantidade,
                    PreVenda = ((VendaResponse)grdVendas.SelectedItem).PreVenda
                };

                var formularioPopup = new FormularioVendasPopup(Venda, jwtToken, "Visualizar");
                formularioPopup.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar formulário do Venda: {ex.Message}");
            }
        }

        private void chbPreVenda_Checked(object sender, RoutedEventArgs e)
        {
            List<VendaResponse> vendasFiltradas = VendaGlobal.vendaGlobal
                .Where(venda => venda.PreVenda == 1)
                .ToList();

            grdVendas.ItemsSource = vendasFiltradas;
        }

        private void chbPreVenda_Unchecked(object sender, RoutedEventArgs e)
        {
            List<VendaResponse> vendasFiltradas = VendaGlobal.vendaGlobal
                .Where(venda => venda.PreVenda == 0)
                .ToList();

            grdVendas.ItemsSource = vendasFiltradas;
        }

    }
}
