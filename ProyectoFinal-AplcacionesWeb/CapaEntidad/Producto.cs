using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidad
{
    public class Producto
    {

        public int IdProducto { get; set; }

        public string NombreProducto { get; set; }

        public string Fotografia { get; set; }

        public string Codigo { get; set; }

        public int IdCategoria { get; set; }

        public int IdProveedor { get; set; }

        public decimal CostoObtenido { get; set; }

        public decimal PrecioVendido { get; set; }

        public int StockActual { get; set; }

        public int StockMinimo { get; set; }

        public bool Estado { get; set; }

        public int TotalVendido { get; set; }

        public Categoria categoria { get; set; }

        public Proveedor proveedor { get; set; }

    }
}
