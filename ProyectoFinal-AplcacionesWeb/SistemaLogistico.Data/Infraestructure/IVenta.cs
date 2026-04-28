using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.Data.Infraestructure
{
    public interface IVenta: IGeneric<Venta>
    {
        public int RegistrarVenta(Venta venta, List<DetalleVenta> detalles, List<DetalleDescuento> descuentos);

        public int AnularVenta(int IdVenta, int IdUsuario);

        public List<Venta> ListadoVentaConFiltro(string Busqueda, string NombreUsuario, bool? Estado);

    }
}
