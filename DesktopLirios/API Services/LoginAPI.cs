﻿using DesktopLirios.Requests;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class LoginAPI
{
    public static async Task<string?> LoginApi(LoginRequest loginRequest)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string uri = "https://localhost:7225/api/Usuario/Login";

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
}