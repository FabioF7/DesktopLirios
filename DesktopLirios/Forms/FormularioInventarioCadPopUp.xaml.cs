using DesktopLirios.Common;
using DesktopLirios.Requests;
using DesktopLirios.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace DesktopLirios
{
    public partial class FormularioInventarioCadPopUp : Window
    {
        private SecureString jwtToken;
        private Dictionary<string, int> estoqueTemporario = new Dictionary<string, int>();
        private List<ProdutoResponse> listaInventario = new List<ProdutoResponse>();
        public InventarioRequest? Inventario { get; set; } = new InventarioRequest();
        private List<KeyValuePair<int?, ProdutoResponse>> listaProdutos;

        public FormularioInventarioCadPopUp(SecureString token)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            jwtToken = token;
            CarregarProdutosAsync();
            txtProdutoInventario.Focus();
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

        private async Task CarregarProdutosAsync()
        {
            try
            {
                var response = await ProdutoAPI.ProdutoApi(null, null, "Get", jwtToken);

                ProdutoGlobal.produtoGlobal = JsonConvert.DeserializeObject<List<ProdutoResponse>>(response);

                listaInventario = ProdutoGlobal.produtoGlobal;

                listaProdutos = ProdutoGlobal.produtoGlobal
                .Select(p => new KeyValuePair<int?, ProdutoResponse>(p.Id, p))
                .ToList();

                ConfigureDataGridColumns();

                grdTodos.ItemsSource = listaInventario;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
            }
        }

        private void ConfigureDataGridColumns()
        {
            grdTodos.AutoGenerateColumns = false;

            grdTodos.Columns.Clear();

            DataGridTextColumn colunaNome = new DataGridTextColumn();
            colunaNome.Header = "Nome";
            colunaNome.Binding = new Binding("Nome");
            grdTodos.Columns.Add(colunaNome);

            DataGridTextColumn colunaQuantidade = new DataGridTextColumn();
            colunaQuantidade.Header = "Previsão";
            colunaQuantidade.Binding = new Binding("Quantidade");
            grdTodos.Columns.Add(colunaQuantidade);

            DataGridTextColumn colunaContabilizado = new DataGridTextColumn();
            colunaContabilizado.Header = "Contabilizado";
            colunaContabilizado.Binding = new Binding("Contabilizado");
            grdTodos.Columns.Add(colunaContabilizado);

            DataGridTextColumn colunaId = new DataGridTextColumn();
            colunaId.Header = "Id";
            colunaId.Binding = new Binding("Id");
            colunaId.Visibility = Visibility.Collapsed;
            grdTodos.Columns.Add(colunaId);

        }

        private void txtProdutoInventario_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string filtro = txtProdutoInventario.Text.Trim();

                var prodSel = listaInventario
                .Where(produto => produto.CodigoDeBarra.ToString().Contains(filtro))
                .Select(produto => produto.Id)
                .FirstOrDefault();

                if (!string.IsNullOrEmpty(filtro))
                {
                    AdicionarProdutoAoEstoqueTemporario(prodSel.ToString());
                    AtualizarExibicaoEstoqueTemporario(prodSel.ToString());

                    grdTodos.ItemsSource = listaInventario;
                    grdTodos.Items.Refresh();

                    txtProdutoInventario.Clear();
                    txtProdutoInventario.Focus();
                }
            }
        }

        private void txtProdutoInventario_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtro = txtProdutoInventario.Text.ToLower();

            if (filtro.Length >= 2)
            {
                lbProduto.Height = Double.NaN;
            }

            var resultadosFiltrados = listaProdutos
                .Where(c => c.Value.Nome.ToLower().Contains(filtro) || c.Value.Codigo.ToString().Contains(filtro) || c.Value.CodigoDeBarra.ToString().Contains(filtro))
                .ToList();

            lbProduto.ItemsSource = resultadosFiltrados;

            lbProduto.Visibility = !string.IsNullOrEmpty(filtro) && resultadosFiltrados.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            Panel.SetZIndex(lbProduto, 1);
        }

        private void lbProduto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbProduto.SelectedItem is KeyValuePair<int?, ProdutoResponse> selectedProduto)
            {
                List<ProdutoResponse> produtoGlobal = ProdutoGlobal.produtoGlobal;

                int selectedId = (int)selectedProduto.Key;

                if (!string.IsNullOrEmpty(selectedId.ToString()))
                {
                    AdicionarProdutoAoEstoqueTemporario(selectedId.ToString());
                    AtualizarExibicaoEstoqueTemporario(selectedId.ToString());

                    grdTodos.ItemsSource = listaInventario;
                    grdTodos.Items.Refresh();

                    txtProdutoInventario.Clear();
                    txtProdutoInventario.Focus();
                }
            }

            lbProduto.Visibility = Visibility.Collapsed;

            Panel.SetZIndex(lbProduto, 0);
        }

        private void AdicionarProdutoAoEstoqueTemporario(string id)
        {
            if (estoqueTemporario.ContainsKey(id))
            {
                estoqueTemporario[id]++;
            }
            else
            {
                estoqueTemporario[id] = 1;
            }
        }

        private void AtualizarExibicaoEstoqueTemporario(string id)
        {
            bool atualizado = false;

            foreach (ProdutoResponse produto in listaInventario)
            {
                if (produto.Id.ToString() == id)
                {
                    produto.Contabilizado = estoqueTemporario.ContainsKey(id) ? estoqueTemporario[id] : 0;
                    atualizado = true;
                    break;
                }
            }

            if (!atualizado)
            {
                MessageBox.Show("Produto não encontrato!");
                estoqueTemporario.Remove(id);
            }

        }

        private async void btnPausa_Click(object sender, RoutedEventArgs e)
        {
            var resultado = MessageBox.Show("Você tem certeza que deseja pausar a contagem do Inventário?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                Inventario.Contado_por = "Admin";
                Inventario.Situacao = "Incompleto";
                Inventario.Inicio = DateTime.Now;

                foreach(int prod in estoqueTemporario.Values)
                {
                    if (Inventario.Contabilizado_Total == null)
                    {
                        Inventario.Contabilizado_Total = prod;
                    }
                    else
                    {
                        Inventario.Contabilizado_Total += prod;
                    }
                }

                try
                {
                    var formularioPopup = new FormularioInventarioInfosPopUp();
                    formularioPopup.ShowDialog();

                    Inventario.Nome = formularioPopup.Nome;
                    Inventario.Obsevacoes = formularioPopup.Obsevacoes;
                    Inventario.Revisado_por = "";

                    var response = await InventarioAPI.InventarioApi(Inventario, null, "Post", jwtToken);

                    if (response != null) {

                        List<InventarioDetalhesRequest> InventarioDetalhes = new List<InventarioDetalhesRequest>();

                        var lista = listaInventario.Where(p => p.Contabilizado > 0);

                        JObject jsonObject = JObject.Parse(response);

                        int idInventario = (int)jsonObject["id"];

                        foreach (var item in lista) {

                            InventarioDetalhesRequest detalhes = new InventarioDetalhesRequest
                            {
                                IdInventario = idInventario,
                                IdProduto = item.Id,
                                Previsao = item.Quantidade,
                                Contabilizado = estoqueTemporario.Where(e => e.Key == item.Id.ToString()).Select(e => e.Value).FirstOrDefault()
                            };

                            InventarioDetalhes.Add(detalhes);
                        }

                        response = await InventarioDetalhesAPI.InventarioDetalhesApi(InventarioDetalhes, null, "Post", jwtToken);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao salvar novo inventário: {ex.Message}");
                }

                this.Close();

            }
        }

        private void btnRevisar_Click(object sender, RoutedEventArgs e)
        {
            var resultado = MessageBox.Show("Você tem certeza que deseja revisar a contagem do Inventário?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {

                //Salva Produtos contados.

            }
        }
    }
}
