using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            //MainFrame.Navigate(new PaginEntradas(jwtToken));
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

    }
}
