using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.Data.Infraestructure
{
    public interface IProducto: IGeneric<Producto>
    {

        public List<MovimientoStock> HistorialMovimientos(int id);

        public List<Producto> ListadoProductoConFiltro(String Busqueda);

    }
}
