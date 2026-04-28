using CapaPresentacion.Models.VM;
using SistemaLogistico.Entities;

namespace CapaPresentacion.Models.Request
{
    public class VentaRequest
    {
        public VentaVM Venta { get; set; }

        public List<DetalleVentaVM> Detalles { get; set; }

        public List<DetalleDescuentoVM> Descuentos { get; set; }

    }
}
