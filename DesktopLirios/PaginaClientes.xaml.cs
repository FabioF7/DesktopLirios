using System;
using System.Collections.Generic;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using DesktopLirios.API_Services;
using DesktopLirios.Responses;
using Newtonsoft.Json;

namespace DesktopLirios
{
    public partial class PaginaClientes : Page
    {
        private SecureString jwtToken;
        public PaginaClientes(SecureString token)
        {
            InitializeComponent();
            jwtToken = token;
        }

        private async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await ClienteAPI.ClienteApi(null, AppConfig.ClienteApiUrl, null, "Get", jwtToken);

                List<ClienteResponse> clientes = JsonConvert.DeserializeObject<List<ClienteResponse>>(response);

                grdClientes.ItemsSource = clientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
            }
        }
    }
}
