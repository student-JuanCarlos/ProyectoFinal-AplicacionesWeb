using SistemaLogistico.Data.Infraestructure;
using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.BussinesLogic.Services
{
    public class ProductoService
    {
        private readonly IProducto productoDB;

        public ProductoService(IProducto service)
        {
            productoDB = service;
        }

        public int GestionarProducto(Producto producto)
        {
            if (producto.IdProducto == 0)
                return productoDB.Agregar(producto);
            else
                return productoDB.Actualizar(producto);
        }

        public List<Producto> ListadoProducto(string Busqueda)
        {
            return productoDB.Listado(Busqueda);
        }

        public Producto DetalleProducto(int id)
        {
            return productoDB.Detalle(id);
        }

        public int CambiarEstadoProducto(int id)
        {
            return productoDB.CambiarEstado(id);
        }

        public List<MovimientoStock> HistorialMovimientoProducto(int id)
        {
            return productoDB.HistorialMovimientos(id);
        }

        public List<Producto> ListadoConFiltro(string Busqueda)
        {
            return productoDB.ListadoProductoConFiltro(Busqueda);
        }
    }
}
