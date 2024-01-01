using Microsoft.Extensions.Configuration;
using System;

namespace DesktopLirios.API_Services
{
    public static class AppConfig
    {
        private static readonly IConfigurationRoot Configuration;
        static AppConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public static string ClienteApiUrl => Configuration.GetSection("ApiSettings:ClienteApiUrl").Value;
        public static string GastosApiUrl => Configuration.GetSection("ApiSettings:GastosApiUrl").Value;
        public static string LoginApiUrl => Configuration.GetSection("ApiSettings:LoginApiUrl").Value;
        public static string OrigemApiUrl => Configuration.GetSection("ApiSettings:OrigemApiUrl").Value;
        public static string PerfilApiUrl => Configuration.GetSection("ApiSettings:PerfilApiUrl").Value;
        public static string PrestadorApiUrl => Configuration.GetSection("ApiSettings:PrestadorApiUrl").Value;
        public static string ProdutoApiUrl => Configuration.GetSection("ApiSettings:ProdutoApiUrl").Value;
        public static string SrvicoApiUrl => Configuration.GetSection("ApiSettings:SrvicoApiUrl").Value;
        public static string TipoServicoApiUrl => Configuration.GetSection("ApiSettings:TipoServicoApiUrl").Value;
        public static string UsuarioApiUrl => Configuration.GetSection("ApiSettings:UsuarioApiUrl").Value;
        public static string VendaApiUrl => Configuration.GetSection("ApiSettings:VendaApiUrl").Value;
    }

}
