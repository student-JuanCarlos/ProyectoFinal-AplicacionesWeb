using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidad
{
    public class MovimientoStock
    {

        public int IdMovimiento { get; set; }

        public string TipoDeMovimiento { get; set; }

        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        public string Motivo { get; set; }

        public string IdUsuario { get; set; }

        public Producto producto { get; set; }

        public Usuario usuario { get; set; }
    }
}
