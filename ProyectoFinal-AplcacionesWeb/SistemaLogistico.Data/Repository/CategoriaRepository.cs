using SistemaLogistico.Data.Infraestructure;
using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SistemaLogistico.Data.Repository
{
    public class CategoriaRepository : ICategoria
    {
        private readonly string cadenaConexion;

        public CategoriaRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:Database"] ?? string.Empty;
        }

        public int Actualizar(Categoria categoria)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Actualizar_Categoria";
                    cmd.Parameters.AddWithValue("@IdCategoria", categoria.IdCategoria);
                    cmd.Parameters.AddWithValue("@NombreCategoria", categoria.NombreCategoria);
                    cmd.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);
                    cn.Open();
                    f = cmd.ExecuteNonQuery();
                    cn.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return f;
        }

        public int Agregar(Categoria categoria)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Insertar_Categoria";
                    cmd.Parameters.AddWithValue("@NombreCategoria", categoria.NombreCategoria);
                    cmd.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);
                    cn.Open();
                    f = cmd.ExecuteNonQuery();
                    cn.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return f;
        }

        #region
        public int CambiarEstado(int id)
        {
            throw new NotImplementedException();
        }

        public Categoria Detalle(int id)
        {
            throw new NotImplementedException();
        }
        #endregion
        public List<Categoria> Listado(string Busqueda)
        {
            List<Categoria> listadoCategorias = new List<Categoria>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Listado_Categoria";
                    cmd.Parameters.AddWithValue("@Busqueda",
                        string.IsNullOrEmpty(Busqueda) ? (object)DBNull.Value : Busqueda);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        listadoCategorias.Add(new Categoria
                        {
                            IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                            NombreCategoria = reader["NombreCategoria"].ToString(),
                            Descripcion = reader["Descripcion"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return listadoCategorias;
        }
    }
}
