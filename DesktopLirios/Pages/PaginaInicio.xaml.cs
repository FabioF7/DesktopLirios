using System.Windows;
using System.Windows.Controls;

namespace DesktopLirios
{
    public partial class PaginaInicio : Page
    {
        public PaginaInicio()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Botão clicado!");
        }
    }
}
