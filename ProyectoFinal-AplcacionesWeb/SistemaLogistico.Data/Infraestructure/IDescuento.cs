using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.Data.Infraestructure
{
    public interface IDescuento: IGeneric<Descuento>
    {
        public List<Descuento> ListadoConFiltro(string Busqueda);

        public List<Descuento> Listado(string Busqueda, bool? Estado);

    }
}
