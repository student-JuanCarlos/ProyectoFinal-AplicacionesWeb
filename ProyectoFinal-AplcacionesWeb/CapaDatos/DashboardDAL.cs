using CapaEntidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CapaDatos
{
    public class DashboardDAL
    {
        private readonly string cadenaConexion = "tuCadenaConexion";
        public (int TotalVentas, decimal TotalIngresos) VentasHoy()
        {
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_VentasHoy";
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return (
                            Convert.ToInt32(reader["TotalVentas"]),
                            Convert.ToDecimal(reader["TotalIngresos"])
                        );
                    }
                    return (0, 0);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public List<Producto> ProductosBajoStock()
        {
            List<Producto> lista = new List<Producto>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ProductosBajoStock";
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Producto
                        {
                            IdProducto = Convert.ToInt32(reader["IdProducto"]),
                            NombreProducto = reader["NombreProducto"].ToString(),
                            StockActual = Convert.ToInt32(reader["StockActual"]),
                            StockMinimo = Convert.ToInt32(reader["StockMinimo"]),
                            categoria = new Categoria
                            {
                                NombreCategoria = reader["NombreCategoria"].ToString()
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return lista;
        }

        public List<Producto> ProductosMasVendidos()
        {
            List<Producto> lista = new List<Producto>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ProductosMasVendidos";
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Producto
                        {
                            NombreProducto = reader["NombreProducto"].ToString(),
                            TotalVendido = Convert.ToInt32(reader["TotalVendido"])
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return lista;
        }

        public List<Venta> UltimasVentas()
        {
            List<Venta> lista = new List<Venta>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_UltimasVentas";
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new Venta
                        {
                            IdVenta = Convert.ToInt32(reader["IdVenta"]),
                            Cliente = reader["Cliente"].ToString(),
                            FechaVenta = Convert.ToDateTime(reader["FechaVenta"]),
                            Total = Convert.ToDecimal(reader["Total"]),
                            usuario = new Usuario
                            {
                                NombreUsuario = reader["NombreUsuario"].ToString()
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return lista;
        }

    }
}
