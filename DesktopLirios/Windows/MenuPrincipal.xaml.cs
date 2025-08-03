using DesktopLirios.Common;
using DesktopLirios.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security;
using System.Windows;
using System.Windows.Controls;

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
            MenuPrincipal_Loaded();
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

        private void MenuPrincipal_Loaded()
        {
            CarregarClientesAsync();
            CarregarProdutosAsync();
            CarregarVendasAsync();
        }

        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainFrame == null) return;

            switch (MenuList.SelectedIndex)
            {
                case 0:
                    MainFrame.Navigate(new PaginaInicio());
                    break;
                case 1:
                    MainFrame.Navigate(new PaginaAgenda(jwtToken));
                    break;
                case 2:
                    MainFrame.Navigate(new PaginaVendas(jwtToken));
                    break;
                case 3:
                    MainFrame.Navigate(new PaginaClientes(jwtToken));
                    break;
                case 4:
                    MainFrame.Navigate(new PaginaServicos(jwtToken));
                    break;
                case 5:
                    MainFrame.Navigate(new PaginaProdutos(jwtToken));
                    break;
                case 6:
                    MainFrame.Navigate(new PaginaGastos(jwtToken));
                    break;
                case 7:
                    //MainFrame.Navigate(new EntradasPage());
                    break;
                case 8:
                    MainFrame.Navigate(new PaginaInventario(jwtToken));
                    break;
                case 9:
                    MainFrame.Navigate(new PaginaRelatorios(jwtToken));
                    break;
                case 10:
                    MainFrame.Navigate(new PaginaOutros(jwtToken));
                    break;
                default:
                    break;
            }

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
                MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
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
                MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
            }
        }

        private async void CarregarVendasAsync()
        {
            try
            {
                var response = await VendaAPI.VendaApi(null, null, "Get", null, jwtToken);

                VendaGlobal.vendaGlobal = JsonConvert.DeserializeObject<List<VendaResponse>>(response);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
            }
        }

    }
}
