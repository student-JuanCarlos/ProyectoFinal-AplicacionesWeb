using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.Data.Infraestructure
{
    public interface IMovimientoStock
    {

        public int RegistrarMovimiento(MovimientoStock movimiento);

        public List<MovimientoStock> ListadoMovimiento(string TipoMovimiento, string Busqueda);

    }
}
