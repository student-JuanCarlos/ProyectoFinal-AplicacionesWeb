using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio
{
    public class ProductoBL
    {

        ProductoDAL productoDAL = new ProductoDAL();

        public int GestionarProducto(Producto producto)
        {
            if (producto.IdProducto == null)
                return productoDAL.InsertarProducto(producto);
            else
                return productoDAL.ActualizarProducto(producto);
        }

        public List<Producto> ListadoProducto(string Busqueda)
        {
            return productoDAL.ListadoProducto(Busqueda);
        }

        public Producto DetalleProducto(int id)
        {
            return productoDAL.DetalleProducto(id);
        }

        public int CambiarEstadoProducto(int id)
        {
            return productoDAL.CambiarEstado(id);
        }

        public List<MovimientoStock> HistorialMovimientoProducto(int id)
        {
            return productoDAL.HistorialMovimientos(id);
        }
    }
}
