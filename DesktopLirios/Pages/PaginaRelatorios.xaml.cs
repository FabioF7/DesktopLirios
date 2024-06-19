using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace DesktopLirios
{
    public partial class PaginaRelatorios : Page
    {
        private SecureString jwtToken;
        public PaginaRelatorios(SecureString token)
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
