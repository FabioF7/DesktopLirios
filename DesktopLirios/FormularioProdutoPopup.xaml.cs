using DesktopLirios.Requests;
using DesktopLirios.Responses;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Windows;
using System.Threading.Tasks;
using System.Security;
using System.Linq;

namespace DesktopLirios
{
    public partial class FormularioProdutoPopup : Window
    {
        public ProdutoRequest? Produto { get; set; }
        private SecureString jwtToken;
        private int id;
        private string tipoTela;

        public FormularioProdutoPopup(ProdutoResponse? ProdutoMostra, SecureString token, string tipoUso)
        {
            InitializeComponent();
            DataContext = this;
            jwtToken = token;
            tipoTela = tipoUso;
            CarregaForm(ProdutoMostra);
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

        private async void CarregaForm(ProdutoResponse? ProdutoMostra)
        {
            cbOrigem.ItemsSource = await CarregaOrigem();

            if (ProdutoMostra != null && tipoTela != "Cadastrar")
            {
                id = (int)ProdutoMostra.Id;
                txtNome.Text = ProdutoMostra.Nome;               
                txtSKU.Text = ProdutoMostra.Codigo;
                txtBarra.Text = ProdutoMostra.CodigoDeBarra.ToString();
                txtCusto.Text = ProdutoMostra.ValorCusto.ToString();
                txtValor.Text = ProdutoMostra.ValorVendaRevista.ToString();
                //cbCategoria.SelectedIndex = ProdutoMostra.IdCategoria;
                txtQtd.Text = ProdutoMostra.Quantidade.ToString();

                if (cbOrigem.ItemsSource != null)
                {
                    cbOrigem.SelectedIndex = ProdutoMostra.OrigemId - 1;
                };

                if (tipoTela == "Editar")
                {
                    Produto = CarregaProduto(ProdutoMostra);
                }
                if (tipoTela == "Visualizar")
                {
                    CarregaVisualizar();
                }
                ProdutoRequest ProdutoEdicao = new ProdutoRequest();

                ProdutoEdicao.Nome = ProdutoMostra.Nome;

                Produto = ProdutoEdicao;
            }
            
        }

        private async void btnSalvar_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (tipoTela == "Editar")
            {
                if (Produto != null)
                {
                    Produto.Nome = txtNome.Text;
                    Produto.OrigemId = cbOrigem.SelectedIndex + 1;
                    Produto.Codigo = txtSKU.Text;
                    Produto.CodigoDeBarra = Int64.Parse(txtBarra.Text);
                    Produto.ValorCusto = long.Parse(txtCusto.Text);
                    Produto.ValorVendaRevista = long.Parse(txtValor.Text);
                    //Produto.IdCategoria = cbCategoria.SelectedIndex;
                    Produto.Quantidade = int.Parse(txtQtd.Text);

                    MessageBoxResult resultado = MessageBox.Show($"Você tem certeza que deseja Salvar as Alterações feitas no Produto {Produto.Nome.ToUpper()}?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resultado == MessageBoxResult.Yes)
                    {
                        try
                        {
                            var response = await ProdutoAPI.ProdutoApi(Produto, id, "Put", jwtToken);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Erro ao alterar Produto");
                }
            }
            else
            {
                MessageBoxResult resultado = MessageBox.Show("Você tem certeza que deseja Salvar o novo Produto?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes)
                {
                    try
                    {
                        Produto = new ProdutoRequest();

                        Produto.Nome = txtNome.Text;
                        Produto.OrigemId = cbOrigem.SelectedIndex + 1;
                        Produto.Codigo = txtSKU.Text;
                        Produto.CodigoDeBarra = Int64.Parse(txtBarra.Text);
                        Produto.ValorCusto = long.Parse(txtCusto.Text);
                        Produto.ValorVendaRevista = long.Parse(txtValor.Text);
                        //Produto.IdCategoria = cbCategoria.SelectedIndex;
                        Produto.Quantidade = int.Parse(txtQtd.Text);

                        var response = await ProdutoAPI.ProdutoApi(Produto, null, "Post", jwtToken);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
                    }
                }             
            }
            Close();
        }

        private ProdutoRequest CarregaProduto(ProdutoResponse? ProdutoMostra)
        {
            ProdutoRequest ProdutoEdicao = new ProdutoRequest();

            ProdutoEdicao.Nome = ProdutoMostra.Nome;
            ProdutoEdicao.OrigemId = ProdutoMostra.OrigemId;
            ProdutoEdicao.Codigo = ProdutoMostra.Codigo;
            ProdutoEdicao.CodigoDeBarra = ProdutoMostra.CodigoDeBarra;
            ProdutoEdicao.ValorCusto = ProdutoMostra.ValorCusto;
            ProdutoEdicao.ValorVendaRevista = ProdutoMostra.ValorVendaRevista;
            ProdutoEdicao.IdCategoria = ProdutoMostra.IdCategoria;
            ProdutoEdicao.Quantidade = ProdutoMostra.Quantidade;

            return ProdutoEdicao;
        }

        private void CarregaVisualizar()
        {
            txtNome.IsReadOnly = true;
            cbOrigem.IsEnabled = false;
            txtSKU.IsReadOnly = true;
            txtBarra.IsReadOnly = true;
            txtCusto.IsReadOnly = true;
            txtValor.IsReadOnly = true;
            cbCategoria.IsEnabled = false;
            txtQtd.IsReadOnly = true;
            btnSalvar.IsEnabled = false;
        }

        private async Task<List<string>> CarregaOrigem()
        {
            var response = await OrigemAPI.OrigemApi(null, null, "Get", jwtToken);
            List<OrigemResponse> origens = JsonConvert.DeserializeObject<List<OrigemResponse>>(response);

            List<string> nomesOrigens = origens.Select(origem => origem.Nome).ToList();

            return nomesOrigens;
        }

    }
}