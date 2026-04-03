using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio
{
    public class UsuarioBL
    {

        UsuarioDAL usuarioDAL = new UsuarioDAL();

        public int GestionarUsuario(Usuario usuario)
        {
            usuario.Contraseña = Utilidades.ConvertirSha256(usuario.Contraseña, usuario.Email);

            if(usuario.IdUsuario == 0)
                return usuarioDAL.RegistroUsuario(usuario);
            else
                return usuarioDAL.ActualizarUsuario(usuario);
        }

        public Usuario LoginUsuario(string Email, string Contraseña)
        {
            string contraseñaHash = Utilidades.ConvertirSha256(Contraseña, Email);

            return usuarioDAL.LoginUsuario(Email, contraseñaHash);
        }

        public List<Usuario> ListadoUsuario(string Busqueda)
        {
            return usuarioDAL.ListadoUsuario(Busqueda);
        }

        public Usuario DetalleUsuario(int id)
        {
            return usuarioDAL.DetalleUsuario(id);
        }

        public int CambiarEstado(int id)
        {
            return usuarioDAL.CambiarEstado(id);
        }

        public int CambiarContraseña(int id, string Email, string contraseñaNueva)
        {
            var usuario = usuarioDAL.DetalleUsuario(id);

            string contraseñaHash = Utilidades.ConvertirSha256(contraseñaNueva, Email);

            return usuarioDAL.CambiarContraseña(id, contraseñaHash);
        }

    }
}
