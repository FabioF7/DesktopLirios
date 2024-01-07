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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopLirios
{
    public partial class PaginaOutros : Page
    {
        private SecureString jwtToken;
        public PaginaOutros(SecureString token)
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
