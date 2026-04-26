using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.Entities
{
    public class DetalleDescuento
    {

        public int IdDetalleDescuento { get; set; }

        public int IdVenta { get; set; }

        public int IdDescuento { get; set; }

        public decimal PorcentajeAplicado { get; set; }

        public Venta Venta { get; set; }

        public Descuento Descuento { get; set; }

    }
}
