using SistemaLogistico.Data.Infraestructure;
using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.BussinesLogic.Services
{
    public class CategoriaService
    {
        private readonly ICategoria categoriaDB;
        
        public CategoriaService(ICategoria service)
        {
            categoriaDB = service;
        }

        public int GestionarCategoria(Categoria categoria)
        {
            if (categoria.IdCategoria == 0)
                return categoriaDB.Agregar(categoria);
            else
                return categoriaDB.Actualizar(categoria);
        }

        public List<Categoria> ListadoCategoria(string Busqueda)
        {
            return categoriaDB.Listado(Busqueda);
        }

    }
}
