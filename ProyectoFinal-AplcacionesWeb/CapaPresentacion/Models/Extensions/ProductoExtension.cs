using CapaPresentacion.Models.VM;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SistemaLogistico.Entities;

namespace CapaPresentacion.Models.Extensions
{
    public static class ProductoExtension
    {

        public static ProductoVM ToViewModel(this Producto producto)
        {
            return new ProductoVM()
            {
                IdProducto = producto.IdProducto,
                NombreProducto = producto.NombreProducto,
                Fotografia = producto.Fotografia,
                Codigo = producto.Codigo,
                IdCategoria = producto.IdCategoria,
                IdProveedor = producto.IdProveedor,
                CostoObtenido = producto.CostoObtenido,
                PrecioVendido = producto.PrecioVendido,
                StockActual = producto.StockActual,
                StockMinimo = producto.StockMinimo,
                Estado = producto.Estado,
                TotalVendido = producto.TotalVendido,
                categoria = new CategoriaVM()
                {
                    IdCategoria = producto.IdCategoria,
                    NombreCategoria = producto.categoria.NombreCategoria,
                },
                proveedor = new ProveedorVM()
                {
                    IdProveedor = producto.IdProveedor,
                    NombreProveedor = producto.proveedor.NombreProveedor
                }
            };
        }

        public static Producto ToEntity(this ProductoVM model)
        {
            return new Producto()
            {
                IdProducto = model.IdProducto,
                NombreProducto = model.NombreProducto,
                Fotografia = model.Fotografia,
                Codigo = model.Codigo,
                IdCategoria = model.IdCategoria,
                IdProveedor = model.IdProveedor,
                CostoObtenido = model.CostoObtenido,
                PrecioVendido = model.PrecioVendido,
                StockActual = model.StockActual,
                StockMinimo = model.StockMinimo,
                Estado = model.Estado,
                TotalVendido = model.TotalVendido
            };
        }

    }
}
