using SistemaLogistico.Entities;

namespace CapaPresentacion.Models.VM
{
    public class MovimientoStockVM
    {

        public int IdMovimiento { get; set; }

        public string TipoDeMovimiento { get; set; }

        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        public string Motivo { get; set; }

        public DateTime FechaMovimiento { get; set; }

        public int IdUsuario { get; set; }

        public ProductoVM producto { get; set; }

        public UsuarioVM usuario { get; set; }

    }
}
