using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.Entities
{
    public class Descuento
    {

        public int IdDescuento { get; set; }

        public string NombreDescuento { get; set; }

        public int IdProducto { get; set; }

        public string TipoDescuento { get; set; }

        public decimal PorcentajeDescuento { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin {  get; set; }

        public bool Estado { get; set; }

        public Producto producto { get; set; }

    }
}
