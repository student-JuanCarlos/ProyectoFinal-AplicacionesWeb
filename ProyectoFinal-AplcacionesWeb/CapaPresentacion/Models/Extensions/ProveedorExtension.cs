using CapaPresentacion.Models.VM;
using SistemaLogistico.Entities;

namespace CapaPresentacion.Models.Extensions
{
    public static class ProveedorExtension
    {

        public static ProveedorVM ToViewModel(this Proveedor proveedor)
        {
            return new ProveedorVM()
            {
                IdProveedor = proveedor.IdProveedor,
                NombreProveedor = proveedor.NombreProveedor,
                RUC = proveedor.RUC,
                Telefono = proveedor.Telefono,
                PaginaWeb = proveedor.PaginaWeb,
                EmailEmpresa = proveedor.EmailEmpresa,
                ProductoOfrecido = proveedor.ProductoOfrecido,
                Estado = proveedor.Estado, 
            };
        }

        public static Proveedor ToEntity(this ProveedorVM model)
        {
            return new Proveedor()
            {
                IdProveedor = model.IdProveedor,
                NombreProveedor = model.NombreProveedor,
                RUC = model.RUC,
                Telefono = model.Telefono,
                PaginaWeb = model.PaginaWeb,
                EmailEmpresa = model.EmailEmpresa,
                ProductoOfrecido = model.ProductoOfrecido,
                Estado = model.Estado,
            };
        }

    }
}
