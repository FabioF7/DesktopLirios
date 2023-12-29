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
        }

        private void LbiInicio_Selected(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PaginaInicio());
        }

        private void LbiAgenda_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void LbiVendas_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void LbiClientes_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void LbiServicos_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void LbiProdutos_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void LbiGastos_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void LbiRelatorios_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void LbiConfiguracoes_Selected(object sender, RoutedEventArgs e)
        {

        }

    }
}
