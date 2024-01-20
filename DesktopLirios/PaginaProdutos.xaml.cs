using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DesktopLirios.API_Services;
using DesktopLirios.Requests;
using DesktopLirios.Responses;
using Newtonsoft.Json;

namespace DesktopLirios
{
    public partial class PaginaProdutos : Page
    {
        private SecureString jwtToken;
        private ProdutoResponse? Produto;
        private List<ProdutoResponse> listaProdutos;

        public PaginaProdutos(SecureString token)
        {
            InitializeComponent();
            jwtToken = token;
            CarregarProdutosAsync();
        }

        private async Task CarregarProdutosAsync()
        {
            try
            {
                var response = await ProdutoAPI.ProdutoApi(null, null, "Get", jwtToken);

                List<ProdutoResponse> produtos = JsonConvert.DeserializeObject<List<ProdutoResponse>>(response);

                listaProdutos = JsonConvert.DeserializeObject<List<ProdutoResponse>>(response);

                grdProdutos.ItemsSource = produtos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
            }
        }

        private void txtPesquisar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string termoPesquisa = txtPesquisar.Text.ToLower();

            List<ProdutoResponse> produtosFiltrados = listaProdutos
            .Where(produto =>
                produto.Nome.ToLower().Contains(termoPesquisa) ||
                produto.Codigo.ToString().ToLower().Contains(termoPesquisa) ||
                produto.CodigoDeBarra.ToString().Contains(termoPesquisa))
            .ToList();

            grdProdutos.ItemsSource = produtosFiltrados;
        }
        
        private async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            await CarregarProdutosAsync();
        }

        private async void btnEditar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Produto = new ProdutoResponse()
                {
                    Id = ((ProdutoResponse)grdProdutos.SelectedItem).Id,
                    Nome = ((ProdutoResponse)grdProdutos.SelectedItem).Nome,
                    OrigemId = ((ProdutoResponse)grdProdutos.SelectedItem).OrigemId,
                    Codigo = ((ProdutoResponse)grdProdutos.SelectedItem).Codigo,
                    CodigoDeBarra = ((ProdutoResponse)grdProdutos.SelectedItem).CodigoDeBarra,
                    ValorCusto = ((ProdutoResponse)grdProdutos.SelectedItem).ValorCusto,
                    ValorVendaRevista = ((ProdutoResponse)grdProdutos.SelectedItem).ValorVendaRevista,
                    IdCategoria = ((ProdutoResponse)grdProdutos.SelectedItem).IdCategoria,
                    Quantidade = ((ProdutoResponse)grdProdutos.SelectedItem).Quantidade
                };

                var formularioPopup = new FormularioProdutoPopup(Produto, jwtToken, "Editar");
                formularioPopup.ShowDialog();

                await CarregarProdutosAsync();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Nenhum Produto selecionado!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar formulário do Produto: {ex.Message}");
            }
        }

        private async void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult resultado = MessageBox.Show($"Você tem certeza que deseja Excluir o Produto {((ProdutoResponse)grdProdutos.SelectedItem).Nome.ToUpper()}?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    var response = await ProdutoAPI.ProdutoApi(null, ((ProdutoResponse)grdProdutos.SelectedItem).Id, "Delete", jwtToken);
                }

                await CarregarProdutosAsync();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Nenhum Produto selecionado!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir dados do Produto: {ex.Message}");
            }
        }

        private async void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var formularioPopup = new FormularioProdutoPopup(null, jwtToken, "Cadastrar");
                formularioPopup.ShowDialog();

                await CarregarProdutosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar formulário de cadastro de Produtos: {ex.Message}");
            }
        }

        private void btnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Produto = new ProdutoResponse()
                {
                    Id = ((ProdutoResponse)grdProdutos.SelectedItem).Id,
                    Nome = ((ProdutoResponse)grdProdutos.SelectedItem).Nome,
                    OrigemId = ((ProdutoResponse)grdProdutos.SelectedItem).OrigemId,
                    Codigo = ((ProdutoResponse)grdProdutos.SelectedItem).Codigo,
                    CodigoDeBarra = ((ProdutoResponse)grdProdutos.SelectedItem).CodigoDeBarra,
                    ValorCusto = ((ProdutoResponse)grdProdutos.SelectedItem).ValorCusto,
                    ValorVendaRevista = ((ProdutoResponse)grdProdutos.SelectedItem).ValorVendaRevista,
                    IdCategoria = ((ProdutoResponse)grdProdutos.SelectedItem).IdCategoria,
                    Quantidade = ((ProdutoResponse)grdProdutos.SelectedItem).Quantidade
                };

                var formularioPopup = new FormularioProdutoPopup(Produto, jwtToken, "Visualizar");
                formularioPopup.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar formulário do Produto: {ex.Message}");
            }
        }
    }
}
