using SistemaLogistico.Data.Infraestructure;
using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.BussinesLogic.Services
{
    public class DashBoardService
    {

        private readonly IDashBoard dashboardDB;

        public DashBoardService(IDashBoard service)
        {
            dashboardDB = service;
        }

        public (int TotalVentas, decimal TotalIngresos) VentasHoy()
        {
            return dashboardDB.VentasHoy();
        }

        public List<Producto> ProductosBajoStock()
        {
            return dashboardDB.ProductosBajoStock();
        }

        public List<Producto> ProductosMasVendidos()
        {
            return dashboardDB.ProductosMasVendidos();
        }

        public List<Venta> UltimasVentas()
        {
            return dashboardDB.UltimasVentas();
        }

    }
}
