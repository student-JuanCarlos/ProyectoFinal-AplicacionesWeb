using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidad.Request
{
    public class VentaRequest
    {

        public Venta Venta { get; set; }
        public List<DetalleVenta> Detalles { get; set; }

    }
}
