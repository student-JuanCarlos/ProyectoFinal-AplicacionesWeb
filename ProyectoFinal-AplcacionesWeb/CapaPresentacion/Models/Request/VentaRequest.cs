using CapaPresentacion.Models.VM;

namespace CapaPresentacion.Models.Request
{
    public class VentaRequest
    {
        public VentaVM Venta { get; set; }

        public List<DetalleVentaVM> Detalles { get; set; }

    }
}
