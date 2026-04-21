using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SistemaLogistico.Data.Infraestructure;
using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SistemaLogistico.Data.Repository
{
    public class ProveedorRepository : IProveedor
    {
        private readonly string cadenaConexion;

        public ProveedorRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:Database"] ?? string.Empty;
        }

        public int Actualizar(Proveedor proveedor)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Actualizar_Proveedor";
                    cmd.Parameters.AddWithValue("@IdProveedor", proveedor.IdProveedor);
                    cmd.Parameters.AddWithValue("@NombreProveedor", proveedor.NombreProveedor);
                    cmd.Parameters.AddWithValue("@RUC", proveedor.RUC);
                    cmd.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                    cmd.Parameters.AddWithValue("@PaginaWeb", proveedor.PaginaWeb);
                    cmd.Parameters.AddWithValue("@EmailEmpresa", proveedor.EmailEmpresa);
                    cmd.Parameters.AddWithValue("@ProductoOfrecido", proveedor.ProductoOfrecido);
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

        public int Agregar(Proveedor proveedor)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Insertar_Proveedor";
                    cmd.Parameters.AddWithValue("@NombreProveedor", proveedor.NombreProveedor);
                    cmd.Parameters.AddWithValue("@RUC", proveedor.RUC);
                    cmd.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                    cmd.Parameters.AddWithValue("@PaginaWeb", proveedor.PaginaWeb);
                    cmd.Parameters.AddWithValue("@EmailEmpresa", proveedor.EmailEmpresa);
                    cmd.Parameters.AddWithValue("@ProductoOfrecido", proveedor.ProductoOfrecido);
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

        public int CambiarEstado(int id)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_CambiarEstado_Proveedor";
                    cmd.Parameters.AddWithValue("@IdProveedor", id);
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

        public Proveedor Detalle(int id)
        {
            Proveedor proveedor = null;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Detalle_Proveedor";
                    cmd.Parameters.AddWithValue("@IdProveedor", id);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        proveedor = new Proveedor()
                        {
                            IdProveedor = Convert.ToInt32(reader["IdProveedor"]),
                            NombreProveedor = reader["NombreProveedor"].ToString(),
                            RUC = reader["RUC"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            PaginaWeb = reader["PaginaWeb"].ToString(),
                            EmailEmpresa = reader["EmailEmpresa"].ToString(),
                            ProductoOfrecido = reader["ProductoOfrecido"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"])
                        };
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return proveedor;
        }

        public List<Proveedor> Listado(string Busqueda)
        {
            List<Proveedor> listadoProveedores = new List<Proveedor>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Listado_Proveedor";
                    cmd.Parameters.AddWithValue("@Busqueda",
                        string.IsNullOrEmpty(Busqueda) ? (object)DBNull.Value : Busqueda);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        listadoProveedores.Add(new Proveedor
                        {
                            IdProveedor = Convert.ToInt32(reader["IdProveedor"]),
                            NombreProveedor = reader["NombreProveedor"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            EmailEmpresa = reader["EmailEmpresa"].ToString(),
                            ProductoOfrecido = reader["ProductoOfrecido"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"])
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return listadoProveedores;
        }

        public List<Proveedor> ListadoConFiltro()
        {
            List<Proveedor> listadoProveedores = new List<Proveedor>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Listado_ProveedorConFiltro";
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        listadoProveedores.Add(new Proveedor
                        {
                            IdProveedor = Convert.ToInt32(reader["IdProveedor"]),
                            NombreProveedor = reader["NombreProveedor"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            EmailEmpresa = reader["EmailEmpresa"].ToString(),
                            ProductoOfrecido = reader["ProductoOfrecido"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"])
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return listadoProveedores;
        }
    }
}
