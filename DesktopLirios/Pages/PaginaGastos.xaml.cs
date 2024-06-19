using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace DesktopLirios
{
    public partial class PaginaGastos : Page
    {
        private SecureString jwtToken;
        public PaginaGastos(SecureString token)
        {
            InitializeComponent();
            jwtToken = token;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Botão clicado!");
        }
    }
}
