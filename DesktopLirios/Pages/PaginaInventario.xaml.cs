using DesktopLirios.Common;
using DesktopLirios.Responses;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DesktopLirios
{
    public partial class PaginaInventario : Page
    {
        private SecureString jwtToken;

        public PaginaInventario(SecureString token)
        {
            InitializeComponent();
            jwtToken = token;
            CarregarInventariosAsync();
        }

        private async Task CarregarInventariosAsync()
        {
            try
            {
                var response = await InventarioAPI.InventarioApi(null, null, "Get", jwtToken);

                InventarioGlobal.inventarioGlobal = JsonConvert.DeserializeObject<List<InventarioResponse>>(response);

                ConfigureDataGridColumns();

                grdTodos.ItemsSource = InventarioGlobal.inventarioGlobal;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados da API: {ex.Message}");
            }
        }

        private void ConfigureDataGridColumns()
        {
            grdTodos.AutoGenerateColumns = false;

            grdTodos.Columns.Clear();

            DataGridTextColumn colunaNome = new DataGridTextColumn();
            colunaNome.Header = "Nome";
            colunaNome.Binding = new Binding("Nome");
            grdTodos.Columns.Add(colunaNome);

            DataGridTextColumn colunaSituacao = new DataGridTextColumn();
            colunaSituacao.Header = "Situação";
            colunaSituacao.Binding = new Binding("Situacao");
            grdTodos.Columns.Add(colunaSituacao);

            DataGridTextColumn colunaInicio = new DataGridTextColumn();
            colunaInicio.Header = "Inicio";
            colunaInicio.Binding = new Binding("Inicio");
            grdTodos.Columns.Add(colunaInicio);

            DataGridTextColumn colunaConcluido = new DataGridTextColumn();
            colunaConcluido.Header = "Concluido";
            colunaConcluido.Binding = new Binding("Concluido");
            grdTodos.Columns.Add(colunaConcluido);

            DataGridTextColumn colunaContado = new DataGridTextColumn();
            colunaContado.Header = "Contado por";
            colunaContado.Binding = new Binding("Contado_por");
            grdTodos.Columns.Add(colunaContado);

            DataGridTextColumn colunaRevisado = new DataGridTextColumn();
            colunaRevisado.Header = "Revisado por";
            colunaRevisado.Binding = new Binding("Revisado_por");
            grdTodos.Columns.Add(colunaRevisado);

            DataGridTextColumn colunaObsevacoes = new DataGridTextColumn();
            colunaObsevacoes.Header = "Obsevações";
            colunaObsevacoes.Binding = new Binding("Obsevacoes");
            grdTodos.Columns.Add(colunaObsevacoes);

            DataGridTextColumn colunaContabilizado = new DataGridTextColumn();
            colunaContabilizado.Header = "Total Contabilizado";
            colunaContabilizado.Binding = new Binding("Contabilizado_Total");
            grdTodos.Columns.Add(colunaContabilizado);

        }

        private async void btnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            await CarregarInventariosAsync();
        }

        private async void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var formularioPopup = new FormularioInventarioCadPopUp(jwtToken, null);
                formularioPopup.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar novo inventario: {ex.Message}");
            }

            await CarregarInventariosAsync();

        }

        private async void btnAbrir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var formularioPopup = new FormularioInventarioCadPopUp(jwtToken, ((InventarioResponse)grdTodos.SelectedItem).Id);
                formularioPopup.ShowDialog();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Nenhum Inventário selecionado!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar novo inventario: {ex.Message}");
            }

            await CarregarInventariosAsync();

        }
    }
}
