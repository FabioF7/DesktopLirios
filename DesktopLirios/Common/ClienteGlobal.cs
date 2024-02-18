using DesktopLirios.Responses;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;

namespace DesktopLirios.Common
{
    public static class ClienteGlobal
    {
        public static List<ClienteResponse>? clienteGlobal { get; set; }

        public static void AdicionarCliente(ClienteResponse cliente)
        {
            if (clienteGlobal == null)
            {
                clienteGlobal = new List<ClienteResponse>();
            }

            clienteGlobal.Add(cliente);
        }

        public static void RemoverClientePorId(int id)
        {
            if (clienteGlobal != null)
            {
                var clienteParaRemover = clienteGlobal.Find(cliente => cliente.Id == id);
                if (clienteParaRemover != null)
                    clienteGlobal.Remove(clienteParaRemover);
            }

        }

        public static async void AtualizarCliente(int id, SecureString jwtToken)
        {
            if (clienteGlobal != null)
            {
                var clienteExistente = clienteGlobal.FindIndex(cliente => cliente.Id == id);

                if (clienteExistente != -1)
                {
                    var response = await ClienteAPI.ClienteApi(null, id, "Get", jwtToken);
                    var clienteAtualizado = JsonConvert.DeserializeObject<ClienteResponse>(response);

                    if (clienteAtualizado != null)
                    {
                        clienteAtualizado.Devido = clienteGlobal[clienteExistente].Devido;

                        float limiteInadimplencia = (float)clienteAtualizado.LimiteInadimplencia;
                        float devido = float.Parse(clienteAtualizado.Devido, CultureInfo.InvariantCulture);
                        float limiteLivre = limiteInadimplencia - devido;

                        if(limiteLivre == 0)
                        {
                            clienteAtualizado.LimiteLivre = clienteAtualizado.LimiteInadimplencia.ToString();
                        }
                        else
                        {
                            clienteAtualizado.LimiteLivre = limiteLivre.ToString("F2");
                        }
                        
                        clienteGlobal[clienteExistente] = clienteAtualizado;
                    }
                }
            }
        }

        public static void AtualizarClientePagamento(int id, float valorPago)
        {
            if (clienteGlobal != null)
            {
                var clienteEditar = ClienteGlobal.clienteGlobal.Find(cliente => cliente.Id == id);

                if (clienteEditar != null)
                {
                    var devido = float.Parse(clienteEditar.Devido, CultureInfo.InvariantCulture);
                    devido -= valorPago;
                    clienteEditar.Devido = devido.ToString();

                    float limiteInadimplencia = (float)clienteEditar.LimiteInadimplencia;
                    float limiteLivre = limiteInadimplencia - devido;
                    clienteEditar.LimiteLivre = limiteLivre.ToString("F2");
                }
            }
        }
    }
}
