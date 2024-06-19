using DesktopLirios.Common;
using DesktopLirios.Requests;
using DesktopLirios.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace DesktopLirios
{
    public partial class FormularioInventarioInfosPopUp : Window
    {
        public string Nome { get; private set; }
        public string? Obsevacoes { get; private set; }

        public FormularioInventarioInfosPopUp()
        {
            InitializeComponent();
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

        private void btnContinuar_Click(object sender, RoutedEventArgs e)
        {
            Nome = txtNomeInventario.Text;
            Obsevacoes = txtObservacao.Text;

            DialogResult = true;
            Close();
        }
    }
}
