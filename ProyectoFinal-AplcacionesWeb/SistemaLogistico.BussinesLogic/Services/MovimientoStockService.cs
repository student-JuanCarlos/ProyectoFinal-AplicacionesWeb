using SistemaLogistico.Data.Infraestructure;
using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.BussinesLogic.Services
{
    public class MovimientoStockService
    {
        private readonly IMovimientoStock movimientostockDB;

        public MovimientoStockService(IMovimientoStock service)
        {
            movimientostockDB = service;
        }

        public int AgregarMovimiento(MovimientoStock movimientostock)
        {
            return movimientostockDB.RegistrarMovimiento(movimientostock);
        }

        public List<MovimientoStock> ListadoMovimiento(string TipoMovimiento, string Busqueda)
        {
            return movimientostockDB.ListadoMovimiento(TipoMovimiento, Busqueda);
        }
    }
}
