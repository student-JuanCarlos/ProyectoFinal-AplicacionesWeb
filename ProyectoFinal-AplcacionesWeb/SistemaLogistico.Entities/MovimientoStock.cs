using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.Entities
{
    public class MovimientoStock
    {

        public int IdMovimiento { get; set; }

        public string TipoDeMovimiento { get; set; }

        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        public string Motivo { get; set; }

        public DateTime FechaMovimiento { get; set; }

        public int IdUsuario { get; set; }

        public Producto producto { get; set; }

        public Usuario usuario { get; set; }

    }
}
