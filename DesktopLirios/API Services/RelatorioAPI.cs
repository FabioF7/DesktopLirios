using DesktopLirios.API_Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading.Tasks;

class RelatorioAPI
{
    public static async Task<string?> relatorioAPI(int? id, string tipoApi, SecureString jwtToken)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppConfig.ObterTokenSecure(jwtToken));

                var content = new StringContent("", Encoding.UTF8, "application/json");

                if (tipoApi == "Get" && id != null)
                    response = await client.GetAsync(string.Format(AppConfig.RelatorioApiUrl, string.Format("Historico/{0}", id)));

                if (response != null && response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    return responseData;
                }
                else
                {
                    Console.WriteLine($"Erro na chamada da API: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na chamada da API: {ex.Message}");
                return null;
            }   
        }
    }
}
