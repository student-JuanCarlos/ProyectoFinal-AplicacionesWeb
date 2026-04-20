using SistemaLogistico.Entities;

namespace CapaPresentacion.Models.VM
{
    public class VentaVM
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

        public UsuarioVM usuario { get; set; }

        public List<DetalleVentaVM> Detalles { get; set; }

    }
}
