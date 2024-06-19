using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace DesktopLirios
{
    public partial class PaginaServicos : Page
    {
        private SecureString jwtToken;
        public PaginaServicos(SecureString token)
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
