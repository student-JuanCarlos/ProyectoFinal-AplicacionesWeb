using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    public class ProductoDAL
    {
        private readonly string cadenaConexion = "Server=localhost;Database=App_Logistica_Inventario;User Id=sa;Password=SaClave24;TrustServerCertificate=True;";
        public int InsertarProducto(Producto producto)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_InsertarProducto";
                    cmd.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto);
                    cmd.Parameters.AddWithValue("@Fotografia", producto.Fotografia);
                    cmd.Parameters.AddWithValue("@Codigo", producto.Codigo);
                    cmd.Parameters.AddWithValue("@CostoObtenido", producto.CostoObtenido);
                    cmd.Parameters.AddWithValue("@PrecioVendido", producto.PrecioVendido);
                    cmd.Parameters.AddWithValue("@Stock", producto.StockActual);
                    cmd.Parameters.AddWithValue("@IdCategoria", producto.IdCategoria);
                    cmd.Parameters.AddWithValue("@IdProveedor", producto.IdProveedor);
                    cn.Open();
                    f = cmd.ExecuteNonQuery();
                    cn.Close();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return f;
        }

        public int ActualizarProducto(Producto producto)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ActualizarProducto";
                    cmd.Parameters.AddWithValue("IdProducto", producto.IdProducto);
                    cmd.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto);
                    cmd.Parameters.AddWithValue("@Fotografia", producto.Fotografia);
                    cmd.Parameters.AddWithValue("@CostoObtenido", producto.CostoObtenido);
                    cmd.Parameters.AddWithValue("@PrecioVendido", producto.PrecioVendido);
                    cmd.Parameters.AddWithValue("@IdCategoria", producto.IdCategoria);
                    cmd.Parameters.AddWithValue("@IdProveedor", producto.IdProveedor);
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

        public List<Producto> ListadoProducto(String Busqueda)
        {
            List<Producto> listadoProductos = new List<Producto>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Listado_Producto";
                    cmd.Parameters.AddWithValue("@Busqueda", string.IsNullOrEmpty(Busqueda) ? (object)DBNull.Value : Busqueda);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Categoria categoria = new Categoria
                        {
                            NombreCategoria = reader["NombreCategoria"].ToString()
                        };

                        listadoProductos.Add(new Producto
                        {
                            IdProducto = Convert.ToInt32(reader["IdProducto"]),
                            NombreProducto = reader["NombreProducto"].ToString(),
                            Fotografia = reader["Fotografia"].ToString(),
                            categoria = categoria,
                            Codigo = reader["Codigo"].ToString(),
                            PrecioVendido = Convert.ToDecimal(reader["PrecioVendido"]),
                            StockActual = Convert.ToInt32(reader["StockActual"].ToString()),
                            Estado = Convert.ToBoolean(reader["Estado"].ToString())
                        });
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return listadoProductos;
        }

        public Producto DetalleProducto(int id)
        {
            Producto producto = null;   
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Detalle_Producto";
                    cmd.Parameters.AddWithValue("@IdProducto", id);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Categoria categoria = new Categoria
                        {
                            IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                            NombreCategoria = reader["NombreCategoria"].ToString()
                        };

                        Proveedor proveedor = new Proveedor
                        {
                            IdProveedor = Convert.ToInt32(reader["IdProveedor"]),
                            NombreProveedor = reader["NombreProveedor"].ToString()
                        };

                        producto = new Producto
                        {
                            IdProducto = Convert.ToInt32(reader["IdProducto"]),
                            NombreProducto = reader["NombreProducto"].ToString(),
                            Fotografia = reader["Fotografia"].ToString(),
                            categoria = categoria,
                            Codigo = reader["Codigo"].ToString(),
                            IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                            IdProveedor = Convert.ToInt32(reader["IdProveedor"]),
                            CostoObtenido = Convert.ToDecimal(reader["CostoObtenido"]),
                            PrecioVendido = Convert.ToDecimal(reader["PrecioVendido"]),
                            proveedor = proveedor,
                            StockActual = Convert.ToInt32(reader["StockActual"].ToString()),
                            Estado = Convert.ToBoolean(reader["Estado"].ToString())
                        };
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return producto;
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
                    cmd.CommandText = "sp_CambiarEstado_Producto";
                    cmd.Parameters.AddWithValue("@IdProducto", id);
                    cn.Open();
                    f = cmd.ExecuteNonQuery();
                    cn.Close();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return f;
        }

        public List<MovimientoStock> HistorialMovimientos(int id)
        {
            List<MovimientoStock> listadoMovimientos = new List<MovimientoStock>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Historial_MovimientosPorProducto";
                    cmd.Parameters.AddWithValue("@IdProducto", id);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Usuario usuario = new Usuario
                        {
                            NombreUsuario = reader["NombreUsuario"].ToString()
                        };

                        DateTime fechaMovimiento = Convert.ToDateTime(reader["FechaMovimiento"]);

                        listadoMovimientos.Add(new MovimientoStock
                        {
                            IdMovimiento = Convert.ToInt32(reader["IdMovimiento"]),
                            TipoDeMovimiento = reader["TipoDeMovimiento"].ToString(),
                            Cantidad = Convert.ToInt32(reader["Cantidad"]),
                            Motivo = reader["Motivo"].ToString(),
                            FechaMovimiento = fechaMovimiento,
                            usuario = usuario
                        });
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return listadoMovimientos;
        }

    }
}
