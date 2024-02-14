using DesktopLirios.Requests;
using DesktopLirios.Responses;
using System;
using System.Windows;
using System.Security;
using System.Threading.Tasks;
using System.Globalization;
using DesktopLirios.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DesktopLirios
{
    public partial class FormularioHistoricoClientePopup : Window
    {
        public ClienteRequest? Cliente { get; set; }
        private SecureString jwtToken;
        private int clienteId;

        public FormularioHistoricoClientePopup(SecureString token, int id)
        {
            InitializeComponent();
            DataContext = this;
            jwtToken = token;
            clienteId = id;
            CarregarHistoricoAsync();
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

        private async Task CarregarHistoricoAsync()
        {
            try
            {
                var response = await RelatorioAPI.relatorioAPI(clienteId, "Get", jwtToken);

                var listaHistorico = JsonConvert.DeserializeObject<List<HistoricoResponse>>(response);

                grdHistorico.ItemsSource = listaHistorico;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
            }
        }

        //private async Task<string> CarregaValorDivida(int id)
        //{
        //    var response = await PagamentoAPI.PagamentoApi(null, id, "Get2", jwtToken);

        //    if(response != null)
        //    {
        //        return response;
        //    }

        //    return null;
        //}
    }
}