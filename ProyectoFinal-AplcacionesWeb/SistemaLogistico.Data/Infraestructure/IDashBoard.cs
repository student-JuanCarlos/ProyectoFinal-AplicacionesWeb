using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.Data.Infraestructure
{
    public interface IDashBoard
    {

        public (int TotalVentas, decimal TotalIngresos) VentasHoy();

        public List<Producto> ProductosBajoStock();

        public List<Producto> ProductosMasVendidos();

        public List<Venta> UltimasVentas();

    }
}
