using DesktopLirios.Requests;
using Microsoft.VisualBasic;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CenterWindowOnScreen();
            PreencherNomeUsuarioSalvo();
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

        private void PreencherNomeUsuarioSalvo()
        {
            string nomeUsuarioSalvo = Properties.Settings.Default.UsuarioSalvo;
            if (!string.IsNullOrEmpty(nomeUsuarioSalvo))
            {
                txUsuario.Text = nomeUsuarioSalvo;
            }
        }

        private async void loginUsuario()
        {
            Properties.Settings.Default.UsuarioSalvo = txUsuario.Text;
            Properties.Settings.Default.Save();

            var response = await LoginAPI.LoginApi(new LoginRequest { Usuario = txUsuario.Text, Senha = txSenha.Password });

            if (response != null)
            {
                SecureString jwtToken = GuardarTokenSecure(response);
                MenuPrincipal menuPrincipal = new MenuPrincipal(jwtToken);
                menuPrincipal.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Senha ou Usuário Incorreto(s)!");
            }
        }

        private SecureString GuardarTokenSecure(string token)
        {
            SecureString jwtToken = new SecureString();

            foreach (char c in token)
            {
                jwtToken.AppendChar(c);
            }

            return jwtToken;
        }

        private async void btnAcessar_Click(object sender, RoutedEventArgs e)
        {
            loginUsuario();
        }
    }
}
