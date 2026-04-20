using SistemaLogistico.Entities;

namespace CapaPresentacion.Models.VM
{
    public class DetalleVentaVM
    {

        public int IdDetalleVenta { get; set; }

        public int IdVenta { get; set; }

        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal SubTotal { get; set; }

        public ProductoVM producto { get; set; }

    }
}
