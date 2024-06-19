using DesktopLirios.Common;
using DesktopLirios.Requests;
using DesktopLirios.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopLirios
{
    public partial class HistoricoClientePopup : Window
    {
        public ClienteRequest? Cliente { get; set; }
        private SecureString jwtToken;
        private int clienteId;

        public HistoricoClientePopup(SecureString token, int id)
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

                var retorno = await CarregaValorDivida(clienteId);

                if (retorno != null)
                {
                    txtTotalDev.Text = retorno.ToString();
                }

                var index = ClienteGlobal.clienteGlobal.FindIndex(cliente => cliente.Id == clienteId);

                txtNome.Text = ClienteGlobal.clienteGlobal[index].Nome;
                txtCelular.Text = ClienteGlobal.clienteGlobal[index].Celular.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
            }
        }

        private void btnGeraCobranca_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async Task<string> CarregaValorDivida(int id)
        {
            var response = await PagamentoAPI.PagamentoApi(null, id, "Get2", jwtToken);

            if (response != null)
            {
                return response;
            }

            return null;
        }
    }
}