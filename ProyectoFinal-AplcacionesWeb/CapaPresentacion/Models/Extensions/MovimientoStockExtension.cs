using CapaPresentacion.Models.VM;
using SistemaLogistico.Entities;

namespace CapaPresentacion.Models.Extensions
{
    public static class MovimientoStockExtension
    {

        public static MovimientoStockVM ToViewModel(this MovimientoStock mv)
        {
            return new MovimientoStockVM()
            {
                IdMovimiento = mv.IdMovimiento,
                TipoDeMovimiento = mv.TipoDeMovimiento,
                IdProducto = mv.IdProducto,
                Cantidad = mv.Cantidad,
                Motivo = mv.Motivo,
                FechaMovimiento = mv.FechaMovimiento,
                IdUsuario = mv.IdUsuario,

                producto = mv.producto != null ? new ProductoVM()
                {
                    IdProducto = mv.IdProducto,
                    NombreProducto = mv.producto.NombreProducto
                } : new ProductoVM() { IdProducto = mv.IdProducto },

                usuario = mv.usuario != null ? new UsuarioVM()
                {
                    IdUsuario = mv.IdUsuario,
                    NombreUsuario = mv.usuario.NombreUsuario
                } : new UsuarioVM() { IdUsuario = mv.IdUsuario }
            };
        }

        public static MovimientoStock ToEntity(this MovimientoStockVM model)
        {
            return new MovimientoStock()
            {
                IdMovimiento = model.IdMovimiento,
                TipoDeMovimiento = model.TipoDeMovimiento,
                IdProducto = model.IdProducto,
                Cantidad = model.Cantidad,
                Motivo = model.Motivo,
                FechaMovimiento = model.FechaMovimiento,
                IdUsuario = model.IdUsuario
            };
        } 
    }
}
