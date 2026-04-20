using SistemaLogistico.Data.Infraestructure;
using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.BussinesLogic.Services
{
    public class RolService
    {
        private readonly IRol rolDB;

        public RolService(IRol service)
        {
            rolDB = service;
        }

        public List<Rol> ListadoRoles()
        {
            return rolDB.Listado();
        }

    }
}
