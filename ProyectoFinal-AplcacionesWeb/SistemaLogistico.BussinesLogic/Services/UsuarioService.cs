using SistemaLogistico.BussinesLogic.Utilidades;
using SistemaLogistico.Data.Infraestructure;
using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.BussinesLogic.Services
{
    public class UsuarioService
    {
        private readonly IUsuario usuarioDB;
        private readonly IUtilidades utilidadesDB;

        public UsuarioService(IUsuario service, IUtilidades utilidadesService)
        {
            usuarioDB = service;
            utilidadesDB = utilidadesService;
        }

        public int GestionarUsuario(Usuario usuario)
        {
            usuario.Contraseña = utilidadesDB.ConvertirSha256(usuario.Contraseña, usuario.Email);

            if (usuario.IdUsuario == 0)
                return usuarioDB.Agregar(usuario);
            else
                return usuarioDB.Actualizar(usuario);
        }

        public Usuario LoginUsuario(string Email, string Contraseña)
        {
            //string contraseñaHash = utilidadesDB.ConvertirSha256(Contraseña, Email);

            return usuarioDB.LoginUsuario(Email, Contraseña);
        }

        public List<Usuario> ListadoUsuario(string Busqueda)
        {
            return usuarioDB.Listado(Busqueda);
        }

        public Usuario DetalleUsuario(int id)
        {
            return usuarioDB.Detalle(id);
        }

        public int CambiarEstado(int id)
        {
            return usuarioDB.CambiarEstado(id);
        }

        public int CambiarContraseña(int id, string Email, string contraseñaNueva)
        {
            var usuario = usuarioDB.Detalle(id);

            string contraseñaHash = utilidadesDB.ConvertirSha256(contraseñaNueva, Email);

            return usuarioDB.CambiarContraseña(id, contraseñaHash);
        }
    }
}
