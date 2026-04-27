using CapaPresentacion.Models.VM;
using SistemaLogistico.Data.Infraestructure;
using SistemaLogistico.Entities;

namespace CapaPresentacion.Models.Extensions
{
    public static class DescuentoExtension
    {
        public static DescuentoVM ToViewModel(this Descuento descuento)
        {
            return new DescuentoVM()
            {
                IdDescuento = descuento.IdDescuento,
                NombreDescuento = descuento.NombreDescuento,
                IdProducto = descuento.IdProducto,
                producto = descuento.producto != null ? new ProductoVM()
                {
                    IdProducto = descuento.producto.IdProducto,
                    NombreProducto = descuento.producto.NombreProducto
                } : null,
                PorcentajeDescuento = descuento.PorcentajeDescuento,
                TipoDescuento = descuento.TipoDescuento,
                FechaInicio = descuento.FechaInicio,
                FechaFin = descuento.FechaFin,
                ColorCard = descuento.ColorCard,
                Estado = descuento.Estado
            };
        }

        public static Descuento ToEntity(this DescuentoVM model)
        {
            return new Descuento()
            {
                IdDescuento = model.IdDescuento,
                NombreDescuento = model.NombreDescuento,
                IdProducto = model.IdProducto,
                PorcentajeDescuento = model.PorcentajeDescuento,
                TipoDescuento = model.TipoDescuento,
                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin,
                ColorCard = model.ColorCard,
                Estado = model.Estado
            };
        }


    }
}
