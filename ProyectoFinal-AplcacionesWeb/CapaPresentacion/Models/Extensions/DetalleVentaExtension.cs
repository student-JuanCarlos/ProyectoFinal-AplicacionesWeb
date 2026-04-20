using CapaPresentacion.Models.VM;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using SistemaLogistico.Entities;

namespace CapaPresentacion.Models.Extensions
{
    public static class DetalleVentaExtension
    {

        public static DetalleVentaVM ToViewModel(this DetalleVenta detalle)
        {
            return new DetalleVentaVM()
            {
                IdDetalleVenta = detalle.IdDetalleVenta,
                IdVenta = detalle.IdVenta,
                Cantidad = detalle.Cantidad,
                IdProducto = detalle.IdProducto,
                PrecioUnitario = detalle.PrecioUnitario,
                SubTotal = detalle.SubTotal,
                producto = new ProductoVM()
                {
                    IdProducto = detalle.IdProducto,
                    Codigo = detalle.producto.Codigo,
                    NombreProducto = detalle.producto.NombreProducto
                }
                
            };
        }

        public static DetalleVenta ToEntity(this DetalleVentaVM model)
        {
            return new DetalleVenta()
            {
                IdDetalleVenta = model.IdDetalleVenta,
                IdVenta = model.IdVenta,
                Cantidad = model.Cantidad,
                IdProducto = model.IdProducto,
                PrecioUnitario = model.PrecioUnitario,
                SubTotal = model.SubTotal
            };
        }

    }
}
