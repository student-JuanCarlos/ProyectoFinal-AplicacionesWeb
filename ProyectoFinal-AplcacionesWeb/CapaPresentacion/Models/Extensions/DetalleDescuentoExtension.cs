using CapaPresentacion.Models.VM;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using SistemaLogistico.Entities;

namespace CapaPresentacion.Models.Extensions
{
    public static class DetalleDescuentoExtension
    {

        public static DetalleDescuentoVM ToViewModel(this DetalleDescuento detalle)
        {
            return new DetalleDescuentoVM()
            {
                IdDetalleDescuento = detalle.IdDescuento,
                IdDescuento = detalle.IdDescuento,
                IdVenta = detalle.IdVenta,
                PorcentajeAplicado = detalle.PorcentajeAplicado,
                Descuento = detalle.Descuento != null ? new DescuentoVM()
                {
                    IdDescuento = detalle.IdDescuento,
                    NombreDescuento = detalle.Descuento.NombreDescuento
                } : null,
                Venta = detalle.Venta != null ? new VentaVM()
                {
                    IdVenta = detalle.IdVenta,
                } : null
            };
        }

        public static DetalleDescuento ToEntity(this DetalleDescuentoVM model)
        {
            return new DetalleDescuento()
            {
                IdDetalleDescuento = model.IdDescuento,
                IdDescuento = model.IdDescuento,
                IdVenta = model.IdVenta,
                PorcentajeAplicado = model.PorcentajeAplicado
            };
        }

    }
}
