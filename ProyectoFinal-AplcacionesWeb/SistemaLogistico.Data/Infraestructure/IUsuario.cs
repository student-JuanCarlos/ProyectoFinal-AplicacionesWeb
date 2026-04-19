using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.Data.Infraestructure
{
    public interface IUsuario: IGeneric<Usuario>
    {

        public Usuario LoginUsuario(string Email, string Contraseña);

        public int CambiarContraseña(int id, string contraseñaNueva);

    }
}
