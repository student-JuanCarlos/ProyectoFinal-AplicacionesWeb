using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio
{
    public class CategoriaBL
    {

        CategoriaDAL categoriaDAL = new CategoriaDAL();

        public int GestionarCategoria(Categoria categoria)
        {
            if (categoria.IdCategoria == 0)
                return categoriaDAL.InsertarCategoria(categoria);
            else
                return categoriaDAL.ActualizarCategoria(categoria);
        }

        public List<Categoria> ListadoCategoria(string Busqueda)
        {
            return categoriaDAL.ListadoCategoria(Busqueda);
        }

    }
}
