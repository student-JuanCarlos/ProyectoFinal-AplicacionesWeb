using CapaPresentacion.Models.VM;
using SistemaLogistico.Entities;

namespace CapaPresentacion.Models.Extensions
{
    public static class RolExtension
    {

        public static RolVM ToViewModel(this Rol rol)
        {
            return new RolVM()
            {
                IdRol = rol.IdRol,
                NombreRol = rol.NombreRol,
            };
        }

        public static Rol ToEntity(this RolVM model)
        {
            return new Rol()
            {
                IdRol = model.IdRol,
                NombreRol = model.NombreRol,
            };
        }

    }
}
