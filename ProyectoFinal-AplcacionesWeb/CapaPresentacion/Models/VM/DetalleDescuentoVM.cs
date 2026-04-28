using SistemaLogistico.Entities;

namespace CapaPresentacion.Models.VM
{
    public class DetalleDescuentoVM
    {

        public int IdDetalleDescuento { get; set; }

        public int IdVenta { get; set; }

        public int IdDescuento { get; set; }

        public decimal PorcentajeAplicado { get; set; }

        public VentaVM Venta { get; set; }

        public DescuentoVM Descuento { get; set; }

    }
}
