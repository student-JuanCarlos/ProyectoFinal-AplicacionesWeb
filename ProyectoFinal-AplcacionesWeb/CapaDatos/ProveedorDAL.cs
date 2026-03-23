using CapaEntidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CapaDatos
{
    public class ProveedorDAL
    {

        public int InsertarProveedor(Proveedor proveedor)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(ConexionBD.cn))
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

        public int ActualizarProveedor(Proveedor proveedor)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(ConexionBD.cn))
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

        public Proveedor DetalleProveedor(int id)
        {
            Proveedor proveedor = null;
            using (SqlConnection cn = new SqlConnection(ConexionBD.cn))
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

        public List<Proveedor> ListadoProveedor(string Busqueda)
        {
            List<Proveedor> listadoProveedores = new List<Proveedor>();
            using (SqlConnection cn = new SqlConnection(ConexionBD.cn))
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

        public int CambiarEstadoProveedor(int id)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(ConexionBD.cn))
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

    }
}
