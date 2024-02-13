using DesktopLirios.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using DesktopLirios.Common;

namespace DesktopLirios
{
    /// <summary>
    /// Lógica interna para MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : Window
    {
        private SecureString jwtToken;
        public MenuPrincipal(SecureString token)
        {
            InitializeComponent();
            jwtToken = token;
            CenterWindowOnScreen();
            CarregarClientesAsync();
            CarregarProdutosAsync();
            CarregarVendasAsync();
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

        private void LbiInicio_Selected(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PaginaInicio());
        }

        private void LbiAgenda_Selected(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PaginaAgenda(jwtToken));
        }

        private void LbiVendas_Selected(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PaginaVendas(jwtToken));
        }

        private void LbiClientes_Selected(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PaginaClientes(jwtToken));
        }

        private void LbiServicos_Selected(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PaginaServicos(jwtToken));
        }

        private void LbiProdutos_Selected(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PaginaProdutos(jwtToken));
        }

        private void LbiGastos_Selected(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PaginaGastos(jwtToken));
        }

        private void LbiEntradas_Selected(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(new PaginaEntradas(jwtToken));
        }

        private void LbiInventario_Selected(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(new PaginaInventario(jwtToken));
        }

        private void LbiRelatorios_Selected(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PaginaRelatorios(jwtToken));
        }

        private void LbiOutros_Selected(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PaginaOutros(jwtToken));
        }

        private async void CarregarClientesAsync()
        {
            try
            {
                var response = await ClienteAPI.ClienteApi(null, null, "Get", jwtToken);

                ClienteGlobal.clienteGlobal = JsonConvert.DeserializeObject<List<ClienteResponse>>(response);

            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
            }
        }

        private async void CarregarProdutosAsync()
        {
            try
            {
                var response = await ProdutoAPI.ProdutoApi(null, null, "Get", jwtToken);

                ProdutoGlobal.produtoGlobal = JsonConvert.DeserializeObject<List<ProdutoResponse>>(response);

            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
            }
        }

        private async void CarregarVendasAsync()
        {
            try
            {
                var response = await VendaAPI.VendaApi(null, null, "Get", jwtToken);

                VendaGlobal.vendaGlobal = JsonConvert.DeserializeObject<List<VendaResponse>>(response);

            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
            }
        }

    }
}
