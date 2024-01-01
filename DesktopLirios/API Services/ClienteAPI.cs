using DesktopLirios.Requests;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class ClienteAPI
{
    public static async Task<string?> ClienteApi(ClienteRequest? clienteRequest, string uri, int? id, string tipoApi, SecureString jwtToken)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ObterTokenSecure(jwtToken));

                string jsonContent = JsonSerializer.Serialize(clienteRequest);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                if (tipoApi == "Post" && id == null)
                {
                   response = await client.PostAsync(uri, content);
                }
                if (tipoApi == "Put" && id != null)
                {
                    uri += uri + id;
                    response = await client.PutAsync(uri, content);
                }
                if (tipoApi == "Delete" && id != null)
                {
                    uri += uri + id;
                    response = await client.DeleteAsync(uri);
                }
                if (tipoApi == "Get" && id == null)
                {
                    response = await client.GetAsync(uri);
                }
                if (tipoApi == "Get" && id != null)
                {
                    uri += uri + id;
                    response = await client.GetAsync(uri);
                }

                if (response.IsSuccessStatusCode)
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
