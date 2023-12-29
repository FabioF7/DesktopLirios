using DesktopLirios.Requests;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class ClienteAPI
{
    public static async Task<string?> LoginApi(LoginRequest loginRequest, string uri, SecureString? jwtToken)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string jsonContent = JsonSerializer.Serialize(loginRequest);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri, content);

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

    private string ObterTokenSecure(SecureString jwtToken)
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
