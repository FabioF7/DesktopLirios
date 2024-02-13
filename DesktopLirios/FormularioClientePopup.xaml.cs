using DesktopLirios.Requests;
using DesktopLirios.Responses;
using System;
using System.Windows;
using System.Security;
using System.Threading.Tasks;

namespace DesktopLirios
{
    public partial class FormularioClientePopup : Window
    {
        public ClienteRequest? Cliente { get; set; }
        private SecureString jwtToken;
        private int id;
        private string tipoTela;

        public FormularioClientePopup(ClienteResponse? ClienteMostra, SecureString token, string tipoUso)
        {
            InitializeComponent();
            DataContext = this;
            jwtToken = token;
            tipoTela = tipoUso;
            CarregaForm(ClienteMostra);
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

        private async void CarregaForm(ClienteResponse? ClienteMostra)
        {
            if (ClienteMostra != null && tipoTela != "Cadastrar")
            {
                id = (int)ClienteMostra.Id;
                txtNome.Text = ClienteMostra.Nome;
                txtEmail.Text = ClienteMostra.Email;
                dpDtNascimento.Text = ClienteMostra.DtNascimento.ToString();
                txtCelular.Text = ClienteMostra.Celular.ToString();
                txtEndereco.Text = ClienteMostra.Endereco;
                txtCEP.Text = ClienteMostra.CEP;
                txtLimite.Text = ClienteMostra.LimiteInadimplencia.ToString();
                txtObservacao.Text = ClienteMostra.Observacoes;
                //txtIndicacao.Text = Cliente.Indicacao;
                cbSim.IsChecked = (ClienteMostra.Inadimplencia == 1);

                if (ClienteMostra.Sexo == 0)
                    cbSexo.SelectedIndex = 0;
                else
                    cbSexo.SelectedIndex = 1;

                if (ClienteMostra.Bloqueado == 0)
                {
                    rbNao.IsChecked = true;
                    rbSim.IsChecked = false;
                }
                else
                {
                    rbSim.IsChecked = true;
                    rbNao.IsChecked = false;
                }

                var retorno = await CarregaValorDivida(id);
                txtValDivida.Text = retorno.ToString();

                if (tipoTela == "Editar")
                {
                    Cliente = CarregaCliente(ClienteMostra);
                }
                if (tipoTela == "Visualizar")
                {
                    CarregaVisualizar();
                }
            }
            else
            {
                btnHistorico.IsEnabled = false;
                btnVenda.IsEnabled = false;
                btnOk.Visibility = Visibility.Collapsed;
                lblPagou.Visibility = Visibility.Collapsed;
                txtPagou.Visibility = Visibility.Collapsed;
            }

        }

        private async void btnSalvar_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (tipoTela == "Editar")
            {
                if (Cliente != null)
                {
                    //API Edita Clientes
                    Cliente.Nome = txtNome.Text;
                    Cliente.Email = txtEmail.Text;
                    Cliente.DtNascimento = dpDtNascimento.SelectedDate;
                    Cliente.Celular = Int64.Parse(txtCelular.Text);
                    Cliente.Endereco = txtEndereco.Text;
                    Cliente.CEP = txtCEP.Text;
                    //txtIndicacao.Text = Cliente.Indicacao;
                    Cliente.Sexo = cbSexo.SelectedIndex;
                    Cliente.Bloqueado = (rbSim.IsChecked == true) ? 1 : 0;
                    Cliente.LimiteInadimplencia = long.Parse(string.IsNullOrEmpty(txtLimite.Text) ? "0" : txtLimite.Text);
                    Cliente.Observacoes = txtObservacao.Text;
                    Cliente.DtAlteracao = DateTime.Now;
                    Cliente.AlteradoPor = "Admin";

                    MessageBoxResult resultado = MessageBox.Show($"Você tem certeza que deseja Salvar as Alterações feitas no Cliente {Cliente.Nome.ToUpper()}?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resultado == MessageBoxResult.Yes)
                    {
                        try
                        {
                            var response = await ClienteAPI.ClienteApi(Cliente, id, "Put", jwtToken);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
                        }
                    }
                }
            }
            if (tipoTela == "Cadastrar")
            {
                MessageBoxResult resultado = MessageBox.Show("Você tem certeza que deseja Salvar o novo Cliente?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes)
                {
                    try
                    {
                        Cliente = new ClienteRequest();

                        Cliente.Nome = txtNome.Text;
                        Cliente.Email = txtEmail.Text;
                        Cliente.DtNascimento = dpDtNascimento.SelectedDate;
                        Cliente.Celular = Int64.Parse(txtCelular.Text);
                        Cliente.Endereco = txtEndereco.Text;
                        Cliente.CEP = txtCEP.Text;
                        //txtIndicacao.Text = Cliente.Indicacao;
                        Cliente.Sexo = cbSexo.SelectedIndex;
                        Cliente.Bloqueado = (rbSim.IsChecked == true) ? 1 : 0;
                        Cliente.LimiteInadimplencia = long.Parse(string.IsNullOrEmpty(txtLimite.Text) ? "0" : txtLimite.Text);
                        Cliente.Observacoes = txtObservacao.Text;
                        Cliente.DtCadastro = DateTime.Now;
                        Cliente.CadastradoPor = "Admin";

                        var response = await ClienteAPI.ClienteApi(Cliente, null, "Post", jwtToken);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
                    }
                }
            }
        }

        private ClienteRequest CarregaCliente(ClienteResponse? ClienteMostra)
        {
            ClienteRequest ClienteEdicao = new ClienteRequest();

            ClienteEdicao.Nome = ClienteMostra.Nome;
            ClienteEdicao.Email = ClienteMostra.Email;
            ClienteEdicao.DtNascimento = ClienteMostra.DtNascimento;
            ClienteEdicao.Celular = ClienteMostra.Celular;
            ClienteEdicao.Endereco = ClienteMostra.Endereco;
            ClienteEdicao.CEP = ClienteMostra.CEP;
            ClienteEdicao.Sexo = ClienteMostra.Sexo;
            ClienteEdicao.Bloqueado = ClienteMostra.Bloqueado;
            ClienteEdicao.LimiteInadimplencia = ClienteMostra.LimiteInadimplencia;
            ClienteEdicao.Observacoes = ClienteMostra.Observacoes;
            //txtIndicacao.Text = Cliente.Indicacao;

            btnHistorico.IsEnabled = false;
            btnVenda.IsEnabled = false;

            return ClienteEdicao;
        }

        private void CarregaVisualizar()
        {
            txtNome.IsReadOnly = true;
            txtEmail.IsReadOnly = true;
            dpDtNascimento.IsEnabled = false;
            txtCelular.IsReadOnly = true;
            txtEndereco.IsReadOnly = true;
            txtCEP.IsReadOnly = true;
            txtValDivida.IsReadOnly = true;
            txtLimite.IsReadOnly = true;
            txtObservacao.IsReadOnly = true;
            //txtIndicacao.Text = Cliente.Indicacao;
            cbSim.IsEnabled = false;
            cbSexo.IsEnabled = false;
            rbSim.IsEnabled = false;
            rbNao.IsEnabled = false;
            btnSalvar.IsEnabled = false;
        }

        private async Task<string> CarregaValorDivida(int id)
        {
            var response = await PagamentoAPI.PagamentoApi(null, id, "Get2", jwtToken);

            if(response != null)
            {
                return response;
            }

            return null;
        }

        private void btnVenda_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VendaResponse venda = new VendaResponse()
                {
                    IdVenda = id
                };

                var formularioPopup = new FormularioVendasPopup(venda, jwtToken, "Cliente");

                formularioPopup.SetTxtClienteSearch(txtNome.Text);

                formularioPopup.ShowDialog();

            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Nenhum Cliente selecionado!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar formulário de Venda: {ex.Message}");
            }

        }

        private void btnHistorico_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //var paginaVendas = new PaginaVendas(jwtToken);
                //MainFrame.Navigate(paginaVendas);

            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Nenhum Cliente selecionado!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar formulário de Venda: {ex.Message}");
            }
        }
    }
}