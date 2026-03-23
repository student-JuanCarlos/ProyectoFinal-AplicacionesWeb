using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio
{
    public class VentaBL
    {

        VentaDAL ventaDAL = new VentaDAL();

        public int RegistroVenta(Venta venta, List<DetalleVenta> detalles)
        {
            return ventaDAL.RegistrarVenta(venta, detalles);
        }

        public Venta DetalleVenta(int id)
        {
            return ventaDAL.DetalleVenta(id);
        }

        public int AnularVenta(int idVenta, int IdUsuario)
        {
            return ventaDAL.AnularVenta(idVenta, IdUsuario);
        }

        public List<Venta> ListadoVentaConFiltro(string Busqueda, string NombreUsuario, bool? Estado)
        {
            return ventaDAL.ListadoVentaConFiltro(Busqueda, NombreUsuario, Estado);
        }

    }
}
