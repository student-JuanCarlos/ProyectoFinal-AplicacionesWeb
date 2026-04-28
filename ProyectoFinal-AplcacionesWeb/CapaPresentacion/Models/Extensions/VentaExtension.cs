
using CapaPresentacion.Models.VM;
using SistemaLogistico.Entities;

namespace CapaPresentacion.Models.Extensions
{
    public static class VentaExtension
    {

        public static VentaVM ToViewModel(this Venta venta)
        {
            return new VentaVM()
            {
                IdVenta = venta.IdVenta,
                Cliente = venta.Cliente,
                DocumentoCliente = venta.DocumentoCliente,
                TelefonoCliente = venta.TelefonoCliente,
                FechaVenta = venta.FechaVenta,
                MetodoPago = venta.MetodoPago,
                Total = venta.Total,
                Estado = venta.Estado,
                IdUsuario = venta.IdUsuario,
                usuario = new UsuarioVM()
                {
                    IdUsuario = venta.IdUsuario,
                    NombreUsuario = venta.usuario.NombreUsuario
                },
                Detalles = venta.Detalles?.Select(d => new DetalleVentaVM()
                {
                    producto = new ProductoVM()
                    {
                        NombreProducto = d.producto.NombreProducto
                    },
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    SubTotal = d.SubTotal
                }).ToList(),
                Descuentos = venta.Descuentos?.Select(d => new DetalleDescuentoVM()
                {
                    Descuento = new DescuentoVM()
                    {
                        NombreDescuento = d.Descuento.NombreDescuento,
                        TipoDescuento = d.Descuento.TipoDescuento
                    },
                    PorcentajeAplicado = d.PorcentajeAplicado
                }).ToList()
            };
        }

        public static Venta ToEntity(this VentaVM model)
        {
            return new Venta()
            {
                IdVenta = model.IdVenta,
                Cliente = model.Cliente,
                DocumentoCliente = model.DocumentoCliente,
                TelefonoCliente = model.TelefonoCliente,
                FechaVenta = model.FechaVenta,
                MetodoPago = model.MetodoPago,
                Total = model.Total,
                Estado = model.Estado,
                IdUsuario = model.IdUsuario,
            };
        }

    }
}
