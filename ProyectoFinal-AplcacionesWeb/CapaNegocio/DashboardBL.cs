using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio
{
    public class DashboardBL
    {

        DashboardDAL dashboardDAL = new DashboardDAL();

        public (int TotalVentas, decimal TotalIngresos) VentasHoy()
        {
            return dashboardDAL.VentasHoy();
        }

        public List<Producto> ProductosBajoStock()
        {
            return dashboardDAL.ProductosBajoStock();
        }

        public List<Producto> ProductosMasVendidos()
        {
            return dashboardDAL.ProductosMasVendidos();
        }

        public List<Venta> UltimasVentas()
        {
            return dashboardDAL.UltimasVentas();
        }

    }
}
