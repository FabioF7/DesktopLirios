﻿using System;

namespace DesktopLirios.Requests
{
    public class PagamentoRequest
    {
        public int ClienteId { get; set; }
        public float ValorPago { get; set; }
        public int TipoTransacao { get; set; }
        public int MetodoPagamento { get; set; }
        public DateTime DtPagamento { get; set; }
        public string? CadastradoPor { get; set; }
    }
}