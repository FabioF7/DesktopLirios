using DesktopLirios.Requests;
using DesktopLirios.Responses;
using System.Collections.Generic;
using System;
using System.Windows;
using System.Security;
using System.Linq;
using System.Windows.Controls;
using DesktopLirios.Common;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Newtonsoft.Json;

namespace DesktopLirios
{
    public partial class FormularioVendasPopup : Window
    {
        public VendaRequest? Venda { get; set; }
        private SecureString jwtToken;
        private int? id;
        private string tipoTela;
        private ObservableCollection<ProdutoResponse> produtosFiltrados = new ObservableCollection<ProdutoResponse>();
        private List<KeyValuePair<int?, ClienteResponse>> listaClientes;
        private List<KeyValuePair<int?, ProdutoResponse>> listaProdutos;

        public FormularioVendasPopup(VendaResponse? VendaMostra, SecureString token, string tipoUso)
        {
            InitializeComponent();
            DataContext = this;
            jwtToken = token;
            tipoTela = tipoUso;
            CarregaForm(VendaMostra);
            CenterWindowOnScreen();
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

        private async void CarregaForm(VendaResponse? VendaMostra)
        {
            if (VendaMostra != null && tipoTela != "Cadastrar")
            {
                id = (int)VendaMostra.Id;

                if (tipoTela == "Editar")
                {
                    Venda = CarregaVenda(VendaMostra);
                    CarregaComboBox();
                }
                if (tipoTela == "Visualizar")
                {
                    CarregaVisualizar();
                }
            }
            else
            {
                CarregaComboBox();
            }

            txtValLucro.IsReadOnly = true;
            txtValTotal.IsReadOnly = true;
        }

        private async void btnSalvar_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (tipoTela == "Editar")
            {
                if (Venda != null)
                {
                    //Venda.Nome = txtNome.Text;


                   // MessageBoxResult resultado = MessageBox.Show($"Você tem certeza que deseja Salvar as Alterações feitas no Venda {Venda.Nome.ToUpper()}?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    //if (resultado == MessageBoxResult.Yes)
                    //{
                    //    try
                    //    {
                    //        var response = await VendaAPI.VendaApi(Venda, id, "Put", jwtToken);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
                    //    }
                    //}
                }
                else
                {
                    MessageBox.Show("Erro ao alterar Venda");
                }
            }
            else
            {
                MessageBoxResult resultado = MessageBox.Show("Você tem certeza que deseja Salvar o novo Venda?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes)
                {
                    try
                    {
                        Venda = new VendaRequest();

                        //Venda.Nome = txtNome.Text;


                        var response = await VendaAPI.VendaApi(Venda, null, "Post", jwtToken);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
                    }
                }             
            }
            Close();
        }

        private void btnExcluirProd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult resultado = MessageBox.Show($"Você tem certeza que deseja Excluir o Produto {((ProdutoResponse)grdProdVenda.SelectedItem).Nome.ToUpper()}?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    var produtoARemover = (ProdutoResponse)grdProdVenda.SelectedItem;
                    produtosFiltrados.Remove(produtoARemover);
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Nenhum Produto selecionado!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir Produto: {ex.Message}");
            }
        }

        private void grdProdVenda_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                if (e.Row.Item is ProdutoResponse produto)
                {
                    var column = e.Column as DataGridTextColumn;

                    if (column != null)
                    {
                        var cellContent = e.EditingElement as TextBox;

                        if (cellContent != null)
                        {
                            if (column.Header == "Quantidade")
                            {
                                var quantidade = int.Parse(cellContent.Text);

                                var produtoEmEstoque = listaProdutos.FirstOrDefault(p => p.Value.Id == produto.Id);

                                if (quantidade > produtoEmEstoque.Value.Quantidade)
                                {
                                    e.Cancel = true;
                                    MessageBox.Show("Quantidade no estoque menor que a quantidade incluída na venda!");
                                }
                                else
                                {
                                    string qtd = cellContent.Text;

                                    CarregaTotaleLucro(produto, null, qtd);
                                }                                
                            }
                            else
                            {
                                string novoValor = cellContent.Text;
                                CarregaTotaleLucro(produto, novoValor, null);
                            }
                        }
                    }
                }
            }
        }

        private VendaRequest CarregaVenda(VendaResponse? VendaMostra)
        {
            VendaRequest VendaEdicao = new VendaRequest();

            //VendaEdicao.Nome = VendaMostra.Nome;


            return VendaEdicao;
        }

        private void CarregaVisualizar()
        {
            //txtNome.IsReadOnly = true;
            //cbOrigem.IsEnabled = false;

        }

        private void CarregaComboBox()
        {
            List<ClienteResponse> clienteGlobal = ClienteGlobal.clienteGlobal;

            listaClientes = clienteGlobal
                .Select(c => new KeyValuePair<int?, ClienteResponse>(c.Id, c))
                .ToList();

            List<ProdutoResponse> produtoGlobal = ProdutoGlobal.produtoGlobal;

            listaProdutos = produtoGlobal
                .Select(p => new KeyValuePair<int?, ProdutoResponse>(p.Id, p))
                .Where(p => p.Value.Quantidade != 0)
                .ToList();
        }

        private void txtClienteSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtro = txtClienteSearch.Text.ToLower();

            if (filtro.Length >= 2)
            {
                lbClientes.Height = Double.NaN;
            }

            var resultadosFiltrados = listaClientes
                .Where(c => c.Value.Nome.ToLower().Contains(filtro) || c.Value.Celular.ToString().Contains(filtro))
                .ToList();

            // Atualizar a fonte de itens da ListBox
            lbClientes.ItemsSource = resultadosFiltrados;

            // Exibir a ListBox se houver texto no TextBox e resultados filtrados
            lbClientes.Visibility = !string.IsNullOrEmpty(filtro) && resultadosFiltrados.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            Panel.SetZIndex(lbClientes, 1);
        }

        private void lbClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbClientes.SelectedItem != null && lbClientes.SelectedItem is KeyValuePair<int?, ClienteResponse> selectedItem)
            {
                id = selectedItem.Key;

                txtClienteSearch.Text = selectedItem.Value.Nome;

                lbClientes.Visibility = Visibility.Collapsed;
                Panel.SetZIndex(lbClientes, 0);
            }
        }

        private void txtProdutoSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtro = txtProdutoSearch.Text.ToLower();

            if (filtro.Length >= 2)
            {
                lbProduto.Height = Double.NaN;
            }

            var resultadosFiltrados = listaProdutos
                .Where(c => c.Value.Nome.ToLower().Contains(filtro) || c.Value.Codigo.ToString().Contains(filtro) || c.Value.CodigoDeBarra.ToString().Contains(filtro))
                .ToList();

            // Atualizar a fonte de itens da ListBox
            lbProduto.ItemsSource = resultadosFiltrados;

            // Exibir a ListBox se houver texto no TextBox e resultados filtrados
            lbProduto.Visibility = !string.IsNullOrEmpty(filtro) && resultadosFiltrados.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            Panel.SetZIndex(lbProduto, 1);
        }

        private void lbProduto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbProduto.SelectedItem is KeyValuePair<int?, ProdutoResponse> selectedProduto)
            {
                List<ProdutoResponse> produtoGlobal = ProdutoGlobal.produtoGlobal;

                int? selectedId = selectedProduto.Key;

                List<ProdutoResponse> produtoSelecionados = produtoGlobal
                    .Where(produto => produto.Id == selectedId)
                    .ToList();

                foreach (var produto in produtoSelecionados)
                {
                    produtosFiltrados.Add(new ProdutoResponse
                    {
                        Id = produto.Id,
                        Quantidade = 1,
                        Nome = produto.Nome,
                        ValorVendaRevista = (float)Math.Round(produto.ValorVendaRevista, 2),
                        Codigo = produto.Codigo,
                        CodigoDeBarra = produto.CodigoDeBarra,
                        ValorCusto = (float)Math.Round(produto.ValorCusto, 2)
                    });
                }

                grdProdVenda.ItemsSource = produtosFiltrados;

                if (grdProdVenda.Items.Count <= 2)
                {
                    ConfigureDataGridColumns();
                }

                CarregaTotaleLucro(null, null, null);
            }

            lbProduto.Visibility = Visibility.Collapsed;

            txtProdutoSearch.Text = string.Empty;

            Panel.SetZIndex(lbProduto, 0);
        }

        private void ConfigureDataGridColumns()
        {
            grdProdVenda.Columns.Clear();

            grdProdVenda.Columns.Add(new DataGridTextColumn { Header = "Id", Binding = new Binding("Id"), IsReadOnly = false, Visibility = Visibility.Collapsed });
            grdProdVenda.Columns.Add(new DataGridTextColumn { Header = "Quantidade", Binding = new Binding("Quantidade"), IsReadOnly = false });
            grdProdVenda.Columns.Add(new DataGridTextColumn { Header = "Nome Produto", Binding = new Binding("Nome"), IsReadOnly = true });
            grdProdVenda.Columns.Add(new DataGridTextColumn { Header = "Valor", Binding = new Binding("ValorVendaRevista"), IsReadOnly = false });
            grdProdVenda.Columns.Add(new DataGridTextColumn { Header = "SKU", Binding = new Binding("Codigo"), IsReadOnly = true });
            grdProdVenda.Columns.Add(new DataGridTextColumn { Header = "Código de Barras", Binding = new Binding("CodigoDeBarra"), IsReadOnly = true });
            grdProdVenda.Columns.Add(new DataGridTextColumn { Header = "Custo", Binding = new Binding("ValorCusto"), IsReadOnly = true });
        }

        private void cbMetPag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtValPag.Visibility = Visibility.Collapsed;
            lblValPag.Visibility = Visibility.Collapsed;

            if (cbMetPag.SelectedIndex != 0)
            {
                txtValPag.Visibility = Visibility.Visible;
                lblValPag.Visibility = Visibility.Visible;
            }
        }

        private void CarregaTotaleLucro(ProdutoResponse? produto, string? novoValor, string? qtd)
        {
            float custo = 0.00f;
            float valorVenda = 0.00f;
            int quantidade = 0;

            foreach (var item in grdProdVenda.Items)
            {

                if (qtd != null && item is ProdutoResponse produtoQtdAtual && produtoQtdAtual == produto)
                {
                    quantidade = int.Parse(qtd);

                    valorVenda += (produto.ValorVendaRevista * quantidade);
                    custo += (produto.ValorCusto * quantidade);
                }
                else if (novoValor != null && item is ProdutoResponse produtoAtual && produtoAtual == produto)
                {
                    produto.ValorVendaRevista = float.Parse(novoValor);

                    valorVenda += produto.ValorVendaRevista;
                    custo += produto.ValorCusto;
                }
                else
                {
                    if (item is ProdutoResponse produtoOutro)
                    {
                        valorVenda += produtoOutro.ValorVendaRevista;
                        custo += produtoOutro.ValorCusto;
                    }
                }
            }

            txtValTotal.Text = valorVenda.ToString("0.00");

            float lucro = valorVenda - custo;
            lucro = (float)Math.Round(lucro, 2);

            txtValLucro.Text = lucro.ToString("0.00");

            if (cbParcelas.IsVisible) {

                List<object> listaParcelas = new List<object>
                {
                    new { Quantidade = 1, ValorParcela = Math.Round((valorVenda), 2) },
                    new { Quantidade = 2, ValorParcela = Math.Round((valorVenda / 2), 2) },
                    new { Quantidade = 3, ValorParcela = Math.Round((valorVenda / 3), 2) },
                    new { Quantidade = 4, ValorParcela = Math.Round((valorVenda / 4), 2) },
                    new { Quantidade = 5, ValorParcela = Math.Round((valorVenda / 5), 2) },
                    new { Quantidade = 6, ValorParcela = Math.Round((valorVenda / 6), 2) },
                    new { Quantidade = 7, ValorParcela = Math.Round((valorVenda / 7), 2) },
                    new { Quantidade = 8, ValorParcela = Math.Round((valorVenda / 8), 2) },
                    new { Quantidade = 9, ValorParcela = Math.Round((valorVenda / 9), 2) },
                    new { Quantidade = 10, ValorParcela = Math.Round((valorVenda / 10), 2) }
                };

                cbParcelas.ItemsSource = listaParcelas;
            }
        }
    }
}