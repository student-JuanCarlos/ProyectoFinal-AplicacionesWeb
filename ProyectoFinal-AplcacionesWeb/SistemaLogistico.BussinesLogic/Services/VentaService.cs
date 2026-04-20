using SistemaLogistico.Data.Infraestructure;
using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.BussinesLogic.Services
{
    public class VentaService
    {
        private readonly IVenta ventaDB;

        public VentaService(IVenta service)
        {
            ventaDB = service;
        }

        public int RegistroVenta(Venta venta, List<DetalleVenta> detalles)
        {
            return ventaDB.RegistrarVenta(venta, detalles);
        }

        public Venta DetalleVenta(int id)
        {
            return ventaDB.Detalle(id);
        }

        public int AnularVenta(int idVenta, int IdUsuario)
        {
            return ventaDB.AnularVenta(idVenta, IdUsuario);
        }

        public List<Venta> ListadoVentaConFiltro(string Busqueda, string NombreUsuario, bool? Estado)
        {
            return ventaDB.ListadoVentaConFiltro(Busqueda, NombreUsuario, Estado);
        }

    }
}
