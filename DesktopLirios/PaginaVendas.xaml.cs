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
    public partial class PaginaVendas : Page
    {
        private SecureString jwtToken;
        private VendaResponse? Venda;
        private List<VendaResponse> listaVendas;

        public PaginaVendas(SecureString token)
        {
            InitializeComponent();
            jwtToken = token;
            CarregarVendasAsync();
        }

        private async Task CarregarVendasAsync()
        {
            try
            {
                var response = await VendaAPI.VendaApi(null, null, "Get", jwtToken);

                List<VendaResponse> Vendas = JsonConvert.DeserializeObject<List<VendaResponse>>(response);

                listaVendas = JsonConvert.DeserializeObject<List<VendaResponse>>(response);

                grdVendas.ItemsSource = Vendas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
            }
        }

        private void txtPesquisar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string termoPesquisa = txtPesquisar.Text.ToLower();

            List<VendaResponse> VendasFiltrados = listaVendas;
            //.Where(Venda =>
            //    Venda.Nome.ToLower().Contains(termoPesquisa) ||
            //    Venda.Codigo.ToString().ToLower().Contains(termoPesquisa) ||
            //    Venda.CodigoDeBarra.ToString().Contains(termoPesquisa))
            //.ToList();

            grdVendas.ItemsSource = VendasFiltrados;
        }
        
        private async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            await CarregarVendasAsync();
        }

        private async void btnEditar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Venda = new VendaResponse()
                {
                    Id = ((VendaResponse)grdVendas.SelectedItem).Id,

                };

                var formularioPopup = new FormularioVendasPopup(Venda, jwtToken, "Editar");
                formularioPopup.ShowDialog();

                await CarregarVendasAsync();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Nenhum Venda selecionado!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar formulário do Venda: {ex.Message}");
            }
        }

        private async void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var formularioPopup = new FormularioVendasPopup(null, jwtToken, "Cadastrar");
                formularioPopup.ShowDialog();

                await CarregarVendasAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar formulário de cadastro de Vendas: {ex.Message}");
            }
        }

        private void btnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Venda = new VendaResponse()
                {
                    Id = ((VendaResponse)grdVendas.SelectedItem).Id,

                };

                var formularioPopup = new FormularioVendasPopup(Venda, jwtToken, "Visualizar");
                formularioPopup.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar formulário do Venda: {ex.Message}");
            }
        }
    }
}
