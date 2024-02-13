using DesktopLirios.API_Services;
using DesktopLirios.Requests;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class PagamentoAPI
{
    public static async Task<string?> PagamentoApi(PagamentoRequest? pagamentoRequest, int? id, string tipoApi, SecureString jwtToken)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppConfig.ObterTokenSecure(jwtToken));

                string jsonContent = JsonSerializer.Serialize(pagamentoRequest);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                if (tipoApi == "Get" && id == null)
                    response = await client.GetAsync(string.Format(AppConfig.PagamentoApiUrl, ""));
                if (tipoApi == "Get2" && id != null)
                    response = await client.GetAsync(string.Format(AppConfig.PagamentoApiUrl, "Divida/" + id));
                if (tipoApi == "Get" && id != null)
                    response = await client.GetAsync(string.Format(AppConfig.PagamentoApiUrl, "Cliente/" + id));
                if (tipoApi == "Post" && id == null)
                    response = await client.PostAsync(string.Format(AppConfig.PagamentoApiUrl, ""), content);
                if (tipoApi == "Put" && id != null)
                    response = await client.PutAsync(string.Format(AppConfig.PagamentoApiUrl, id), content);
                if (tipoApi == "Delete" && id != null) 
                    response = await client.DeleteAsync(string.Format(AppConfig.PagamentoApiUrl, id));

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
