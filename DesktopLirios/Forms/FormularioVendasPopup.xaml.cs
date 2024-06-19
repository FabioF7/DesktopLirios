using DesktopLirios.Common;
using DesktopLirios.Requests;
using DesktopLirios.Responses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DesktopLirios
{
    public partial class FormularioVendasPopup : Window
    {
        public VendaRequest? Venda { get; set; }
        private SecureString jwtToken;
        private int? idCliente;
        private int idVenda;
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
            if (VendaMostra != null && tipoTela == "Cliente")
            {
                idVenda = VendaMostra.IdVenda;

                CarregaComboBox();
            }
            else if (VendaMostra != null && tipoTela != "Cadastrar")
            {
                idVenda = VendaMostra.IdVenda;

                txtClienteSearch.TextChanged -= txtClienteSearch_TextChanged;

                txtClienteSearch.Text = ClienteGlobal.clienteGlobal
                                        .Where(cliente => cliente.Id == VendaMostra.ClienteId)
                                        .Select(cliente => cliente.Nome)
                                        .FirstOrDefault();

                txtClienteSearch.TextChanged += txtClienteSearch_TextChanged;

                chbPreVenda.IsChecked = VendaMostra.PreVenda == 1 ? true : false;
                cbMetPag.SelectedIndex = VendaMostra.MetodoPagamento;
                //txtValPag.Text = VendaMostra.MetodoPagamento;

                ConfigureDataGridColumns();

                grdProdVenda.ItemsSource = VendaGlobal.vendaGlobal
                                            .Where(venda => venda.IdVenda == VendaMostra.IdVenda)
                                            .ToList();

                float custo = 0.00f;
                float valorVenda = 0.00f;

                foreach (var produto in grdProdVenda.ItemsSource as IEnumerable<VendaResponse>)
                {
                    valorVenda += produto.ValorVenda * produto.Quantidade;
                    custo += produto.CustoProduto * produto.Quantidade;
                };

                txtValTotal.Text = valorVenda.ToString("F2");

                float lucro = valorVenda - custo;

                txtValLucro.Text = lucro.ToString("F2");

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

        private void ConfigureDataGridColumns()
        {
            grdProdVenda.AutoGenerateColumns = false;

            grdProdVenda.Columns.Clear();

            DataGridTextColumn colunaVenda = new DataGridTextColumn();
            colunaVenda.Header = "Id Venda";
            colunaVenda.Binding = new Binding("IdVenda");
            grdProdVenda.Columns.Add(colunaVenda);

            DataGridTextColumn colunaCliente = new DataGridTextColumn();
            colunaCliente.Header = "Cliente";
            colunaCliente.Binding = new Binding("Cliente.Nome");
            grdProdVenda.Columns.Add(colunaCliente);

            DataGridTextColumn colunaProduto = new DataGridTextColumn();
            colunaProduto.Header = "Produto";
            colunaProduto.Binding = new Binding("Produto.Nome");
            grdProdVenda.Columns.Add(colunaProduto);

            DataGridTextColumn colunaValorVenda = new DataGridTextColumn();
            colunaValorVenda.Header = "Valor da Venda";
            colunaValorVenda.Binding = new Binding("ValorVenda") { StringFormat = "{0:0.00}", ConverterCulture = new CultureInfo("pt-BR") };
            grdProdVenda.Columns.Add(colunaValorVenda);

            DataGridTextColumn colunaCusto = new DataGridTextColumn();
            colunaCusto.Header = "Custo Produto";
            colunaCusto.Binding = new Binding("CustoProduto") { StringFormat = "{0:0.00}", ConverterCulture = new CultureInfo("pt-BR") };
            grdProdVenda.Columns.Add(colunaCusto);

            DataGridTextColumn colunaQuantidade = new DataGridTextColumn();
            colunaQuantidade.Header = "Quantidade";
            colunaQuantidade.Binding = new Binding("Quantidade");
            grdProdVenda.Columns.Add(colunaQuantidade);

            DataGridTextColumn colunaData = new DataGridTextColumn();
            colunaData.Header = "Data Venda";
            colunaData.Binding = new Binding("DtVenda") { StringFormat = "dd/MM/yyyy" };
            grdProdVenda.Columns.Add(colunaData);

            DataGridTextColumn colunaMetodo = new DataGridTextColumn();
            colunaMetodo.Header = "Método de Pagamento";
            colunaMetodo.Binding = new Binding("MetodoPagamento")
            {
                Converter = new MetodoPagamentoConverter()
            };
            grdProdVenda.Columns.Add(colunaMetodo);

            DataGridTextColumn colunaTipo = new DataGridTextColumn();
            colunaTipo.Header = "Tipo";
            colunaTipo.Binding = new Binding("Tipo");
            grdProdVenda.Columns.Add(colunaTipo);

            DataGridTextColumn colunaTransacao = new DataGridTextColumn();
            colunaTransacao.Header = "Tipo Transação";
            colunaTransacao.Binding = new Binding("TipoTransacao");
            grdProdVenda.Columns.Add(colunaTransacao);
        }

        private VendaRequest CarregaVenda(VendaResponse? VendaMostra)
        {
            VendaRequest VendaEdicao = new VendaRequest();

            VendaEdicao.IdVenda = idVenda;
            VendaEdicao.ValorVenda = VendaMostra.ValorVenda;
            VendaEdicao.DtVenda = VendaMostra.DtVenda;
            VendaEdicao.ClienteId = VendaMostra.ClienteId;
            VendaEdicao.ProdutoId = VendaMostra.ProdutoId;
            VendaEdicao.CustoProduto = VendaMostra.CustoProduto;
            VendaEdicao.MetodoPagamento = VendaMostra.MetodoPagamento;
            VendaEdicao.Tipo = VendaMostra.Tipo;
            VendaEdicao.TipoTransacao = VendaMostra.TipoTransacao;
            VendaEdicao.Lucro = VendaMostra.Lucro;
            VendaEdicao.Quantidade = VendaMostra.Quantidade;
            VendaEdicao.PreVenda = VendaMostra.PreVenda;

            return VendaEdicao;
        }

        private void CarregaVisualizar()
        {
            txtClienteSearch.IsReadOnly = true;
            txtProdutoSearch.IsReadOnly = true;
            txtValPag.IsReadOnly = true;
            btnExcluirProd.IsEnabled = false;
            txtValTotal.IsReadOnly = true;
            cbMetPag.IsEnabled = false;
            txtValLucro.IsReadOnly = true;
            btnSalvar.IsEnabled = false;
            chbPreVenda.IsEnabled = false;
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
                idCliente = selectedItem.Key;

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
                    ConfigureDataGridColumnsProduto();
                }

                CarregaTotaleLucro(null, null, null);
            }

            lbProduto.Visibility = Visibility.Collapsed;

            txtProdutoSearch.Text = string.Empty;

            Panel.SetZIndex(lbProduto, 0);
        }

        private async void btnSalvar_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                if (produtosFiltrados.Count > 0)
                {
                    List<VendaRequest> vendas = new List<VendaRequest>();

                    foreach (var produto in produtosFiltrados)
                    {
                        VendaRequest venda = new VendaRequest
                        {
                            ValorVenda = produto.ValorVendaRevista,
                            DtVenda = DateTime.Now,
                            ClienteId = (int)idCliente,
                            ProdutoId = produto.Id,
                            CustoProduto = produto.ValorCusto,
                            MetodoPagamento = cbMetPag.SelectedIndex,
                            Tipo = 0,
                            TipoTransacao = 0,
                            Lucro = float.Parse(txtValLucro.Text),
                            Quantidade = produto.Quantidade,
                            PreVenda = (bool)chbPreVenda.IsChecked ? 1 : 0,
                            CadastradoPor = "fabio.firmino"
                        };

                        vendas.Add(venda);
                    }

                    MessageBoxResult resultado;

                    if (tipoTela == "Editar")
                    {
                        resultado = MessageBox.Show("Você tem certeza que deseja Salvar as Alterações feitas na Venda?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (resultado == MessageBoxResult.Yes)
                        {
                            var response = await VendaAPI.VendaApi(vendas, idVenda, "Post", null, jwtToken);

                            if (response != null)
                            {
                                MessageBox.Show("Venda cadastrada com Sucesso!.", "Venda Finalizada", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {
                        resultado = MessageBox.Show("Você tem certeza que deseja Salvar a nova venda?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (resultado == MessageBoxResult.Yes)
                        {
                            var response = await VendaAPI.VendaApi(vendas, null, "Post", txtValPag.Text, jwtToken);

                            if (response != null)
                            {
                                MessageBox.Show("Venda cadastrada com Sucesso!.", "Venda Finalizada", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Nenhum produto na venda. Adicione pelo menos um produto para salvar a venda.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
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
                    produto.ValorVendaRevista = float.Parse(novoValor, CultureInfo.InvariantCulture);

                    valorVenda += produto.ValorVendaRevista * produto.Quantidade;
                    custo += produto.ValorCusto * produto.Quantidade;
                }
                else
                {
                    if (item is ProdutoResponse produtoOutro)
                    {
                        valorVenda += (produtoOutro.ValorVendaRevista * produtoOutro.Quantidade);
                        custo += (produtoOutro.ValorCusto * produtoOutro.Quantidade);
                    }
                }
            }

            txtValTotal.Text = valorVenda.ToString("0.00");

            float lucro = valorVenda - custo;
            lucro = (float)Math.Round(lucro, 2);

            txtValLucro.Text = lucro.ToString("0.00");

            if (cbParcelas.IsVisible)
            {

                List<object> listaParcelas = new List<object>
                {
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

        private void ConfigureDataGridColumnsProduto()
        {
            grdProdVenda.AutoGenerateColumns = false;

            grdProdVenda.Columns.Clear();

            grdProdVenda.Columns.Add(new DataGridTextColumn { Header = "Id", Binding = new Binding("Id"), IsReadOnly = false, Visibility = Visibility.Collapsed });
            grdProdVenda.Columns.Add(new DataGridTextColumn { Header = "Quantidade", Binding = new Binding("Quantidade"), IsReadOnly = false });
            grdProdVenda.Columns.Add(new DataGridTextColumn { Header = "Nome Produto", Binding = new Binding("Nome"), IsReadOnly = true });
            grdProdVenda.Columns.Add(new DataGridTextColumn { Header = "Valor", Binding = new Binding("ValorVendaRevista") { StringFormat = "{0:0.00}", ConverterCulture = new CultureInfo("pt-BR") }, IsReadOnly = false });
            grdProdVenda.Columns.Add(new DataGridTextColumn { Header = "SKU", Binding = new Binding("Codigo"), IsReadOnly = true });
            grdProdVenda.Columns.Add(new DataGridTextColumn { Header = "Código de Barras", Binding = new Binding("CodigoDeBarra"), IsReadOnly = true });
            grdProdVenda.Columns.Add(new DataGridTextColumn { Header = "Custo", Binding = new Binding("ValorCusto") { StringFormat = "{0:0.00}", ConverterCulture = new CultureInfo("pt-BR") }, IsReadOnly = true });
        }

        private void cbMetPag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtValPag.Visibility = Visibility.Collapsed;
            lblValPag.Visibility = Visibility.Collapsed;
            cbParcelas.Visibility = Visibility.Collapsed;
            lbParcelas.Visibility = Visibility.Collapsed;

            if (cbMetPag.SelectedIndex != 0)
            {
                txtValPag.Visibility = Visibility.Visible;
                lblValPag.Visibility = Visibility.Visible;
            }

            if (cbMetPag.SelectedIndex == 5 || cbMetPag.SelectedIndex == 6)
            {
                cbParcelas.Visibility = Visibility.Visible;
                lbParcelas.Visibility = Visibility.Visible;

                CarregaTotaleLucro(null, null, null);
            }
        }

        public void SetTxtClienteSearch(string value)
        {
            txtClienteSearch.Text = value;
        }

        private void chbPreVenda_Checked(object sender, RoutedEventArgs e)
        {
            List<ProdutoResponse> produtoGlobal = ProdutoGlobal.produtoGlobal;

            listaProdutos = produtoGlobal
                .Select(p => new KeyValuePair<int?, ProdutoResponse>(p.Id, p)).ToList();

        }

        private void chbPreVenda_Unchecked(object sender, RoutedEventArgs e)
        {
            List<ProdutoResponse> produtoGlobal = ProdutoGlobal.produtoGlobal;

            listaProdutos = produtoGlobal
                .Select(p => new KeyValuePair<int?, ProdutoResponse>(p.Id, p))
                .Where(p => p.Value.Quantidade != 0)
                .ToList();

        }
    }
}