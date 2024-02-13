using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DesktopLirios.API_Services;
using DesktopLirios.Common;
using DesktopLirios.Requests;
using DesktopLirios.Responses;
using Newtonsoft.Json;

namespace DesktopLirios
{
    public partial class PaginaClientes : Page
    {
        private SecureString jwtToken;
        private ClienteResponse? Cliente;
        private List<ClienteResponse> listaClientes;

        public PaginaClientes(SecureString token)
        {
            InitializeComponent();
            jwtToken = token;
            CarregarClientesAsync();
        }

        private async Task CarregarClientesAsync()
        {
            try
            {
                var response = await ClienteAPI.ClienteApi(null, null, "Get", jwtToken);

                List<ClienteResponse> clientes = JsonConvert.DeserializeObject<List<ClienteResponse>>(response);

                ClienteGlobal.clienteGlobal = JsonConvert.DeserializeObject<List<ClienteResponse>>(response);
                
                listaClientes = JsonConvert.DeserializeObject<List<ClienteResponse>>(response);

                grdClientes.ItemsSource = clientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
            }
        }

        private void txtPesquisar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string termoPesquisa = txtPesquisar.Text.ToLower();

            List<ClienteResponse> clientesFiltrados = listaClientes
            .Where(cliente =>
                cliente.Nome.ToLower().Contains(termoPesquisa) ||
                cliente.Celular.ToString().Contains(termoPesquisa))
            .ToList();

            grdClientes.ItemsSource = clientesFiltrados;
        }

        private void cbInad_Checked(object sender, RoutedEventArgs e)
        {
            List<ClienteResponse> clientesFiltrados = listaClientes
                .Where(cliente => cliente.Inadimplencia == 1)
                .ToList();

            grdClientes.ItemsSource = clientesFiltrados;
        }

        private void cbInad_Unchecked(object sender, RoutedEventArgs e)
        {
            List<ClienteResponse> clientesFiltrados = listaClientes
                .Where(cliente => cliente.Inadimplencia == 0)
                .ToList();

            grdClientes.ItemsSource = clientesFiltrados;
        }
        
        private async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            await CarregarClientesAsync();
        }

        private async void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente = new ClienteResponse()
                {
                    Id = ((ClienteResponse)grdClientes.SelectedItem).Id,
                    Nome = ((ClienteResponse)grdClientes.SelectedItem).Nome,
                    Email = ((ClienteResponse)grdClientes.SelectedItem).Email,
                    DtNascimento = ((ClienteResponse)grdClientes.SelectedItem).DtNascimento,
                    Celular = ((ClienteResponse)grdClientes.SelectedItem).Celular,
                    Endereco = ((ClienteResponse)grdClientes.SelectedItem).Endereco,
                    CEP = ((ClienteResponse)grdClientes.SelectedItem).CEP,
                    Indicacao = ((ClienteResponse)grdClientes.SelectedItem).Indicacao,
                    Sexo = ((ClienteResponse)grdClientes.SelectedItem).Sexo,
                    Bloqueado = ((ClienteResponse)grdClientes.SelectedItem).Bloqueado,
                    Inadimplencia = ((ClienteResponse)grdClientes.SelectedItem).Inadimplencia,
                    LimiteInadimplencia = ((ClienteResponse)grdClientes.SelectedItem).LimiteInadimplencia,
                    Observacoes = ((ClienteResponse)grdClientes.SelectedItem).Observacoes
                };

                var formularioPopup = new FormularioClientePopup(Cliente, jwtToken, "Editar");
                formularioPopup.ShowDialog();

                await CarregarClientesAsync();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Nenhum Cliente selecionado!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar formulário do Cliente: {ex.Message}");
            }
        }

        private async void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult resultado = MessageBox.Show($"Você tem certeza que deseja Excluir o Cliente {((ClienteResponse)grdClientes.SelectedItem).Nome.ToUpper()}?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    var response = await ClienteAPI.ClienteApi(null, ((ClienteResponse)grdClientes.SelectedItem).Id, "Delete", jwtToken);
                }

                await CarregarClientesAsync();
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
            try
            {
                var formularioPopup = new FormularioClientePopup(null, jwtToken, "Cadastrar");
                formularioPopup.ShowDialog();

                await CarregarClientesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar formulário do Cliente: {ex.Message}");
            }
        }

        private void btnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente = new ClienteResponse()
                {
                    Id = ((ClienteResponse)grdClientes.SelectedItem).Id,
                    Nome = ((ClienteResponse)grdClientes.SelectedItem).Nome,
                    Email = ((ClienteResponse)grdClientes.SelectedItem).Email,
                    DtNascimento = ((ClienteResponse)grdClientes.SelectedItem).DtNascimento,
                    Celular = ((ClienteResponse)grdClientes.SelectedItem).Celular,
                    Endereco = ((ClienteResponse)grdClientes.SelectedItem).Endereco,
                    CEP = ((ClienteResponse)grdClientes.SelectedItem).CEP,
                    Indicacao = ((ClienteResponse)grdClientes.SelectedItem).Indicacao,
                    Sexo = ((ClienteResponse)grdClientes.SelectedItem).Sexo,
                    Bloqueado = ((ClienteResponse)grdClientes.SelectedItem).Bloqueado,
                    Inadimplencia = ((ClienteResponse)grdClientes.SelectedItem).Inadimplencia,
                    LimiteInadimplencia = ((ClienteResponse)grdClientes.SelectedItem).LimiteInadimplencia,
                    Observacoes = ((ClienteResponse)grdClientes.SelectedItem).Observacoes
                };

                var formularioPopup = new FormularioClientePopup(Cliente, jwtToken, "Visualizar");
                formularioPopup.ShowDialog();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Nenhum Cliente selecionado!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar formulário do Cliente: {ex.Message}");
            }
        }
    }
}
