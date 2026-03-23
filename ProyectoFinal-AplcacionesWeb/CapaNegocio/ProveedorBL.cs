using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio
{
    public class ProveedorBL
    {

        ProveedorDAL proveedorDAL = new ProveedorDAL();

        public int GestionarProveedor(Proveedor proveedor)
        {
            if (proveedor.IdProveedor == 0)
                return proveedorDAL.InsertarProveedor(proveedor);
            else
                return proveedorDAL.ActualizarProveedor(proveedor)
        }

        public List<Proveedor> ListadoProveedor(string Busqueda)
        {
            return proveedorDAL.ListadoProveedor(Busqueda);
        }

        public Proveedor DetalleProveedor(int id)
        {
            return proveedorDAL.DetalleProveedor(id);
        }

        public int CambiarEstadoProveedor(int id)
        {
            return proveedorDAL.CambiarEstadoProveedor(id);
        }

    }
}
