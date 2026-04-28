using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.Entities
{
    public class Venta
    {

        public int IdVenta { get; set; }

        public string Cliente { get; set; }

        public string DocumentoCliente { get; set; }

        public string TelefonoCliente { get; set; }

        public DateTime FechaVenta { get; set; }

        public string MetodoPago { get; set; }

        public decimal Total { get; set; }

        public bool Estado { get; set; }

        public int IdUsuario { get; set; }

        public Usuario usuario { get; set; }

        public List<DetalleVenta> Detalles { get; set; }

        public List<DetalleDescuento> Descuentos { get; set; }

    }
}
