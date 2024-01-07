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

                List<ProdutoResponse> clientes = JsonConvert.DeserializeObject<List<ProdutoResponse>>(response);

                listaProdutos = JsonConvert.DeserializeObject<List<ProdutoResponse>>(response);

                grdProdutos.ItemsSource = clientes;
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
            .Where(produto => produto.Nome.ToLower().Contains(termoPesquisa))
            .ToList();

            grdProdutos.ItemsSource = produtosFiltrados;
        }
        
        private async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            await CarregarProdutosAsync();
        }

        private async void btnEditar_Click(object sender, RoutedEventArgs e)
        {

        //    try
        //    {
        //        Produto = new ProdutoResponse()
        //        {
        //            Id = ((ProdutoResponse)grdProdutos.SelectedItem).Id,
        //            Nome = ((ProdutoResponse)grdProdutos.SelectedItem).Nome,

        //        };

        //        var formularioPopup = new FormularioProdutoPopup(Produto, jwtToken);
        //        formularioPopup.ShowDialog();

        //        await CarregarProdutosAsync();
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        MessageBox.Show("Nenhum Cliente selecionado!");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Erro ao carregar formulário do Cliente: {ex.Message}");
        //    }
        }

        private async void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult resultado = MessageBox.Show($"Você tem certeza que deseja Excluir o Cliente {((ClienteResponse)grdProdutos.SelectedItem).Nome.ToUpper()}?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    var response = await ClienteAPI.ClienteApi(null, ((ClienteResponse)grdProdutos.SelectedItem).Id, "Delete", jwtToken);
                }

                await CarregarProdutosAsync();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Nenhum Cliente selecionado!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir dados do Cliente: {ex.Message}");
            }
        }

        private async void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
        //    try
        //    {
        //        var formularioPopup = new FormularioProdutoPopup(null, jwtToken);
        //        formularioPopup.ShowDialog();

        //        await CarregarProdutosAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Erro ao carregar formulário do Cliente: {ex.Message}");
        //    }
        }

        private void btnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            //    try
            //    {
            //        var formularioPopup = new FormularioProdutoPopup(null, jwtToken);
            //        formularioPopup.ShowDialog();

            //        await CarregarProdutosAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show($"Erro ao carregar formulário do Cliente: {ex.Message}");
            //    }
        }
    }
}
