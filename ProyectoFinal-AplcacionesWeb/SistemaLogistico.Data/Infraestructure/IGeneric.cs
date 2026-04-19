using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaLogistico.Data.Infraestructure
{
    public interface IGeneric<Entity> where Entity : class
    {

        public int Agregar(Entity entity);

        public int Actualizar(Entity entity);

        public List<Entity> Listado(String Busqueda);

        public int CambiarEstado(int id);

        public Entity Detalle(int id);
    }
}
