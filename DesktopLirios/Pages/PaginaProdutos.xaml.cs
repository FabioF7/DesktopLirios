using DesktopLirios.Common;
using DesktopLirios.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DesktopLirios
{
    public partial class PaginaProdutos : Page
    {
        private SecureString jwtToken;
        private ProdutoResponse? Produto;

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

                ProdutoGlobal.produtoGlobal = JsonConvert.DeserializeObject<List<ProdutoResponse>>(response);

                ConfigureDataGridColumns();

                grdProdutos.ItemsSource = ProdutoGlobal.produtoGlobal;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
            }
        }

        private void ConfigureDataGridColumns()
        {
            grdProdutos.AutoGenerateColumns = false;

            grdProdutos.Columns.Clear();

            DataGridTextColumn colunaId = new DataGridTextColumn();
            colunaId.Header = "Id Produto";
            colunaId.Binding = new Binding("Id");
            grdProdutos.Columns.Add(colunaId);

            DataGridTextColumn colunaNome = new DataGridTextColumn();
            colunaNome.Header = "Nome";
            colunaNome.Binding = new Binding("Nome");
            grdProdutos.Columns.Add(colunaNome);

            DataGridTextColumn colunaCusto = new DataGridTextColumn();
            colunaCusto.Header = "Valor Custo";
            colunaCusto.Binding = new Binding("ValorCusto") { StringFormat = "{0:0.00}", ConverterCulture = new CultureInfo("pt-BR") };
            grdProdutos.Columns.Add(colunaCusto);

            DataGridTextColumn colunaValorVenda = new DataGridTextColumn();
            colunaValorVenda.Header = "Valor Venda";
            colunaValorVenda.Binding = new Binding("ValorVendaRevista") { StringFormat = "{0:0.00}", ConverterCulture = new CultureInfo("pt-BR") };
            grdProdutos.Columns.Add(colunaValorVenda);

            DataGridTextColumn colunaQuantidade = new DataGridTextColumn();
            colunaQuantidade.Header = "Quantidade";
            colunaQuantidade.Binding = new Binding("Quantidade");
            grdProdutos.Columns.Add(colunaQuantidade);

            DataGridTextColumn colunaOrigem = new DataGridTextColumn();
            colunaOrigem.Header = "Origem";
            colunaOrigem.Binding = new Binding("OrigemId")
            {
                Converter = new OrigemConverter()
            };
            grdProdutos.Columns.Add(colunaOrigem);

            DataGridTextColumn colunaSKU = new DataGridTextColumn();
            colunaSKU.Header = "SKU";
            colunaSKU.Binding = new Binding("Codigo");
            grdProdutos.Columns.Add(colunaSKU);

            DataGridTextColumn colunaBarra = new DataGridTextColumn();
            colunaBarra.Header = "Codigo de Barra";
            colunaBarra.Binding = new Binding("CodigoDeBarra");
            grdProdutos.Columns.Add(colunaBarra);

            DataGridTextColumn colunaCategoria = new DataGridTextColumn();
            colunaCategoria.Header = "Categoria";
            colunaCategoria.Binding = new Binding("IdCategoria");
            {
                //Converter = new CategoriaConverter()
            };
            grdProdutos.Columns.Add(colunaCategoria);

            DataGridTextColumn colunaAtivo = new DataGridTextColumn();
            colunaAtivo.Header = "Ativo";
            colunaAtivo.Binding = new Binding("Ativo");
            grdProdutos.Columns.Add(colunaAtivo);
        }

        private void txtPesquisar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string termoPesquisa = txtPesquisar.Text.ToLower();

            List<ProdutoResponse> produtosFiltrados = ProdutoGlobal.produtoGlobal
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
