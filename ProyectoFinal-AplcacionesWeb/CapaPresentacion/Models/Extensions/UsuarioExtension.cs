using CapaPresentacion.Models.VM;
using SistemaLogistico.Entities;
using System.Linq.Expressions;

namespace CapaPresentacion.Models.Extensions
{
    public static class UsuarioExtension
    {

        public static UsuarioVM ToViewModel(this Usuario usuario)
        {
            return new UsuarioVM()
            {
                IdUsuario = usuario.IdUsuario,
                NombreUsuario = usuario.NombreUsuario,
                IdRol = usuario.IdRol,
                Documento = usuario.Documento,
                Telefono = usuario.Telefono,
                Email = usuario.Email,
                Contraseña = usuario.Contraseña,
                FechaCreacion = usuario.FechaCreacion,
                Estado = usuario.Estado,
                rol = new RolVM()
                {
                    IdRol = usuario.IdRol,
                    NombreRol = usuario.rol.NombreRol
                }
            };
        }

        public static Usuario ToEntity(this UsuarioVM model)
        {
            return new Usuario()
            {
                IdUsuario = model.IdUsuario,
                NombreUsuario = model.NombreUsuario,
                IdRol = model.IdRol,
                Documento = model.Documento,
                Telefono = model.Telefono,
                Email = model.Email,
                Contraseña = model.Contraseña,
                FechaCreacion = model.FechaCreacion,
                Estado = model.Estado
            };
        }

    }
}
