using SistemaLogistico.Data.Infraestructure;
using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace SistemaLogistico.Data.Repository
{
    public class DescuentoRepository : IDescuento
    {
        private readonly string cadenaConexion;

        public DescuentoRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:Database"] ?? string.Empty;
        }

        public int Actualizar(Descuento descuento)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ActualizarDescuento";
                    cmd.Parameters.AddWithValue("@IdDescuento", descuento.IdDescuento);
                    cmd.Parameters.AddWithValue("@NombreDescuento", descuento.NombreDescuento);
                    cmd.Parameters.AddWithValue("@IdProducto", descuento.IdProducto);
                    cmd.Parameters.AddWithValue("@TipoDescuento", descuento.TipoDescuento);
                    cmd.Parameters.AddWithValue("@PorcentajeDescuento", descuento.PorcentajeDescuento);
                    cmd.Parameters.AddWithValue("@FechaInicio", descuento.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", descuento.FechaFin);
                    cmd.Parameters.AddWithValue("@ColorCard", descuento.ColorCard);
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

        public int Agregar(Descuento descuento)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_InsertarDescuento";
                    cmd.Parameters.AddWithValue("@NombreDescuento", descuento.NombreDescuento);
                    cmd.Parameters.AddWithValue("@IdProducto", descuento.IdProducto == 0 ? (object)DBNull.Value : descuento.IdProducto);
                    cmd.Parameters.AddWithValue("@TipoDescuento", descuento.TipoDescuento);
                    cmd.Parameters.AddWithValue("@PorcentajeDescuento", descuento.PorcentajeDescuento);
                    cmd.Parameters.AddWithValue("@FechaInicio", descuento.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", descuento.FechaFin);
                    cmd.Parameters.AddWithValue("@ColorCard", descuento.ColorCard);
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
                    cmd.CommandText = "sp_CambiarEstadoDescuento";
                    cmd.Parameters.AddWithValue("@IdDescuento", id);
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

        public Descuento Detalle(int id)
        {
            var descuento = new Descuento();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_DetalleDescuento";
                    cmd.Parameters.AddWithValue("@IdDescuento", id);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Producto producto = reader["IdProducto"] == DBNull.Value ? null : new Producto()
                        {
                            IdProducto = Convert.ToInt32(reader["IdProducto"]),
                            NombreProducto = reader["NombreProducto"].ToString()
                        };

                        DateTime fechaInicio = Convert.ToDateTime(reader["FechaInicio"]);
                        DateTime fechaFin = Convert.ToDateTime(reader["FechaFin"]);

                        return new Descuento()
                        {
                            IdDescuento = Convert.ToInt32(reader["IdDescuento"]),
                            NombreDescuento = reader["NombreDescuento"].ToString(),
                            IdProducto = reader["IdProducto"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdProducto"]),
                            producto = producto,
                            TipoDescuento = reader["TipoDescuento"].ToString(),
                            PorcentajeDescuento = Convert.ToDecimal(reader["PorcentajeDescuento"]),
                            FechaInicio = fechaInicio,
                            FechaFin = fechaFin,
                            ColorCard = reader["ColorCard"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"])
                        };
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return descuento;
        }

        public List<Descuento> Listado(string Busqueda)
        {
            var listado = new List<Descuento>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ListadoDescuento";
                    cmd.Parameters.AddWithValue("@Busqueda", string.IsNullOrEmpty(Busqueda) ? (object)DBNull.Value : Busqueda);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Producto producto = reader["IdProducto"] == DBNull.Value ? null : new Producto()
                        {
                            NombreProducto = reader["NombreProducto"].ToString()
                        };

                        DateTime fechaFin = Convert.ToDateTime(reader["FechaFin"]);

                        listado.Add(new Descuento()
                        {
                            IdDescuento = Convert.ToInt32(reader["IdDescuento"]),
                            NombreDescuento = reader["NombreDescuento"].ToString(),
                            producto = producto,
                            PorcentajeDescuento = Convert.ToDecimal(reader["PorcentajeDescuento"]),
                            FechaFin = fechaFin,
                            ColorCard = reader["ColorCard"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"])
                        });
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return listado;
        }

        public List<Descuento> ListadoConFiltro(string Busqueda)
        {
            var listado = new List<Descuento>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ListadoDescuentoConFiltro";
                    cmd.Parameters.AddWithValue("@Busqueda", string.IsNullOrEmpty(Busqueda) ? (object)DBNull.Value : Busqueda);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Producto producto = reader["IdProducto"] == DBNull.Value ? null : new Producto()
                        {
                            NombreProducto = reader["NombreProducto"].ToString()
                        };

                        DateTime fechaFin = Convert.ToDateTime(reader["FechaFin"]);

                        listado.Add(new Descuento()
                        {
                            IdDescuento = Convert.ToInt32(reader["IdDescuento"]),
                            NombreDescuento = reader["NombreDescuento"].ToString(),
                            producto = producto,
                            PorcentajeDescuento = Convert.ToDecimal(reader["PorcentajeDescuento"]),
                            FechaFin = fechaFin,
                            ColorCard = reader["ColorCard"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"])
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return listado;
        }
    }
}
