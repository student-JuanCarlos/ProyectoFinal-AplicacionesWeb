using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio
{
    public class MovimientoStockBL
    {

        MovimientoStockDAL movimientostockDAL = new MovimientoStockDAL();

        public int AgregarMovimiento(MovimientoStock movimientostock)
        {
            return movimientostockDAL.RegistrarMovimiento(movimientostock);
        }

        public List<MovimientoStock> ListadoMovimiento(string TipoMovimiento, string Busqueda)
        {
            return movimientostockDAL.ListadoMovimiento(TipoMovimiento, Busqueda);
        }

    }
}
