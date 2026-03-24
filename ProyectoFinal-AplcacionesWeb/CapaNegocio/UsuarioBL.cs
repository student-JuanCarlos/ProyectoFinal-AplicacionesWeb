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
            if(usuario.IdUsuario == 0)
                return usuarioDAL.RegistroUsuario(usuario);
            else
                return usuarioDAL.ActualizarUsuario(usuario);
        }

        public Usuario LoginUsuario(string Email, string Contraseña)
        {
            return usuarioDAL.LoginUsuario(Email, Contraseña);
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

    }
}
