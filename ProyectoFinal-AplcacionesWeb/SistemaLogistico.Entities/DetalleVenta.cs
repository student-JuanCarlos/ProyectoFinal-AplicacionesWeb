using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.Entities
{
    public class DetalleVenta
    {

        public int IdDetalleVenta { get; set; }

        public int IdVenta { get; set; }

        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal SubTotal { get; set; }

        public Producto producto { get; set; }

    }
}
