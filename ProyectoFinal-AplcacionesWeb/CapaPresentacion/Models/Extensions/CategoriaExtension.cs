using CapaPresentacion.Models.VM;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SistemaLogistico.Entities;

namespace CapaPresentacion.Models.Extensions
{
    public static class CategoriaExtension
    {
        public static CategoriaVM ToViewModel(this Categoria categoria)
        {
            return new CategoriaVM()
            {
                IdCategoria = categoria.IdCategoria,
                NombreCategoria = categoria.NombreCategoria,
                Descripcion = categoria.Descripcion,
            };
        }

        public static Categoria ToEntity(this CategoriaVM model)
        {
            return new Categoria()
            {
                IdCategoria = model.IdCategoria,
                NombreCategoria = model.NombreCategoria,
                Descripcion = model.Descripcion,
            };
        }

    }
}
