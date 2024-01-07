using DesktopLirios.Requests;
using DesktopLirios.Responses;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Security;

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
            tipoUso = tipoUso;
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

        private void CarregaForm(ProdutoResponse? ProdutoMostra)
        {
            if (ProdutoMostra != null)
            {
                id = (int)ProdutoMostra.Id;
                txtNome.Text = ProdutoMostra.Nome;

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
    }
}