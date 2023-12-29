using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopLirios.Requests
{
        public class LoginRequest
        {
            public string? Usuario { get; set; } = string.Empty;
            public string? Senha { get; set; } = string.Empty;
        }
}