using SistemaLogistico.Data.Infraestructure;
using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.BussinesLogic.Services
{
    public class ProveedorService
    {
        private readonly IProveedor proveedorDB;

        public ProveedorService(IProveedor service)
        {
            proveedorDB = service;
        }

        public int GestionarProveedor(Proveedor proveedor)
        {
            if (proveedor.IdProveedor == 0)
                return proveedorDB.Agregar(proveedor);
            else
                return proveedorDB.Actualizar(proveedor);
        }

        public List<Proveedor> ListadoProveedor(string Busqueda)
        {
            return proveedorDB.Listado(Busqueda);
        }

        public Proveedor DetalleProveedor(int id)
        {
            return proveedorDB.Detalle(id);
        }

        public int CambiarEstadoProveedor(int id)
        {
            return proveedorDB.CambiarEstado(id);
        }
    }
}
