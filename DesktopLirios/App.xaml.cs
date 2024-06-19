using DesktopLirios.Common;
using System.Windows;

namespace DesktopLirios
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            ClienteGlobal.clienteGlobal = null;
            ProdutoGlobal.produtoGlobal = null;
            VendaGlobal.vendaGlobal = null;
        }
    }
}
