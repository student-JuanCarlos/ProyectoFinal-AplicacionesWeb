using SistemaLogistico.Data.Infraestructure;
using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.BussinesLogic.Services
{
    public class DescuentoService
    {
        private readonly IDescuento descuentoDB;

        public DescuentoService(IDescuento service)
        {
            descuentoDB = service;
        }

        public int GestionarDescuento(Descuento descuento)
        {
            if (descuento.IdDescuento == 0)
                return descuentoDB.Agregar(descuento);
            else
                return descuentoDB.Actualizar(descuento);
        }

        public List<Descuento> ListadoDescuento(string Busqueda)
        {
            return descuentoDB.Listado(Busqueda);
        }

        public List<Descuento> ListadoDescuentoConFiltro(string Busqueda)
        {
            return descuentoDB.ListadoConFiltro(Busqueda);
        }

        public Descuento DetalleDescuento(int id)
        {
            return descuentoDB.Detalle(id);
        }

        public int CambiarEstadoDescuento(int id)
        {
            return descuentoDB.CambiarEstado(id);
        }
    }
}
