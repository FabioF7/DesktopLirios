using DesktopLirios.API_Services;
using DesktopLirios.Requests;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class VendaAPI
{
    public static async Task<string?> VendaApi(VendaRequest? VendaRequest, int? id, string tipoApi, SecureString jwtToken)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ObterTokenSecure(jwtToken));

                string jsonContent = JsonSerializer.Serialize(VendaRequest);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                if (tipoApi == "Get" && id == null)
                    response = await client.GetAsync(string.Format(AppConfig.VendaApiUrl, ""));
                if (tipoApi == "Get" && id != null)
                    response = await client.GetAsync(string.Format(AppConfig.VendaApiUrl, id));
                if (tipoApi == "Get2" && id != null)
                    response = await client.GetAsync(string.Format(AppConfig.VendaApiUrl, "Cliente/" + id));
                if (tipoApi == "Post" && id == null)
                    response = await client.PostAsync(string.Format(AppConfig.VendaApiUrl,""), content);
                if (tipoApi == "Put" && id != null)
                    response = await client.PutAsync(string.Format(AppConfig.VendaApiUrl, id), content);
                if (tipoApi == "Delete" && id != null) 
                    response = await client.DeleteAsync(string.Format(AppConfig.VendaApiUrl, id));

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

    private static string ObterTokenSecure(SecureString jwtToken)
    {
        IntPtr valuePtr = IntPtr.Zero;

        try
        {
            valuePtr = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(jwtToken);
            return System.Runtime.InteropServices.Marshal.PtrToStringUni(valuePtr);
        }
        finally
        {
            System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
        }
    }
}
