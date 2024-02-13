using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DesktopLirios.Common;
using DesktopLirios.Responses;
using Newtonsoft.Json;

namespace DesktopLirios
{
    public partial class PaginaVendas : Page
    {
        private SecureString jwtToken;
        private VendaResponse? Venda;
        private List<VendaResponse> listaVendas;

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
                var response = await VendaAPI.VendaApi(null, null, "Get", jwtToken);

                List<VendaResponse> Vendas = JsonConvert.DeserializeObject<List<VendaResponse>>(response);

                VendaGlobal.vendaGlobal = JsonConvert.DeserializeObject<List<VendaResponse>>(response);

                listaVendas = JsonConvert.DeserializeObject<List<VendaResponse>>(response);

                grdVendas.ItemsSource = Vendas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
            }
        }

        private void txtPesquisar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string termoPesquisa = txtPesquisar.Text.ToLower();

            //List<VendaResponse> VendasFiltrados = listaVendas
            //.Where(Venda =>
            //    Venda.Nome.ToLower().Contains(termoPesquisa) ||
            //    Venda.Codigo.ToString().ToLower().Contains(termoPesquisa)
            //.ToList();

            //grdVendas.ItemsSource = VendasFiltrados;
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
            List<VendaResponse> vendasFiltradas = listaVendas
                .Where(venda => venda.PreVenda == 1)
                .ToList();

            grdVendas.ItemsSource = vendasFiltradas;
        }

        private void chbPreVenda_Unchecked(object sender, RoutedEventArgs e)
        {
            List<VendaResponse> vendasFiltradas = listaVendas
                .Where(venda => venda.PreVenda == 0)
                .ToList();

            grdVendas.ItemsSource = vendasFiltradas;
        }

    }
}
