using CapaEntidad;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CapaDatos
{
    public class VentaDAL
    {
        private readonly string cadenaConexion = "Server=localhost;Database=App_Logistica_Inventario;User Id=sa;Password=SaClave24;TrustServerCertificate=True;";
        public int RegistrarVenta(Venta venta, List<DetalleVenta> detalles)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("IdProducto", typeof(int));
                    dt.Columns.Add("Cantidad", typeof(int));

                    foreach (var d in detalles)
                    {
                        dt.Rows.Add(d.IdProducto, d.Cantidad);
                    }

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_RegistrarVenta";
                    cmd.Parameters.AddWithValue("@Cliente", venta.Cliente);
                    cmd.Parameters.AddWithValue("@DocumentoCliente", venta.DocumentoCliente);
                    cmd.Parameters.AddWithValue("@TelefonoCliente", venta.TelefonoCliente);
                    cmd.Parameters.AddWithValue("@MetodoPago", venta.MetodoPago);
                    cmd.Parameters.AddWithValue("@IdUsuario", venta.IdUsuario);

                    SqlParameter tvp = cmd.Parameters.AddWithValue("@Detalle", dt);
                    tvp.SqlDbType = SqlDbType.Structured;
                    tvp.TypeName = "TVP_DetalleVenta";

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

        public Venta DetalleVenta(int id)
        {
            Venta venta = null;
            using(SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Detalle_Venta";
                    cmd.Parameters.AddWithValue("@IdVenta", id);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Usuario usuario = new Usuario()
                        {
                            NombreUsuario = reader["NombreUsuario"].ToString()
                        };

                        DateTime fechaVenta = Convert.ToDateTime(reader["FechaVenta"]);

                        venta = new Venta()
                        {
                            IdVenta = Convert.ToInt32(reader["IdVenta"]),
                            Cliente = reader["Cliente"].ToString(),
                            DocumentoCliente = reader["DocumentoCliente"].ToString(),
                            TelefonoCliente = reader["TelefonoCliente"].ToString(),
                            MetodoPago = reader["MetodoPago"].ToString(),
                            FechaVenta = fechaVenta,
                            Total = Convert.ToDecimal(reader["Total"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            usuario = usuario,
                            Detalles = new List<DetalleVenta>()
                        };

                        reader.NextResult();
                        while (reader.Read())
                        {
                            venta.Detalles.Add(new DetalleVenta
                            {
                                producto = new Producto()
                                {
                                    NombreProducto = reader["NombreProducto"].ToString(),
                                    Codigo = reader["Codigo"].ToString()
                                },

                                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                PrecioUnitario = Convert.ToDecimal(reader["PrecioUnitario"]),
                                SubTotal = Convert.ToDecimal(reader["SubTotal"])
                            });
                        }

                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return venta;
        }

        public int AnularVenta(int IdVenta, int IdUsuario)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_AnularVenta";
                    cmd.Parameters.AddWithValue("@IdVenta", IdVenta);
                    cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
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

        public List<Venta> ListadoVentaConFiltro(string Busqueda, string NombreUsuario, bool? Estado)
        {
            List<Venta> listadoVentas = new List<Venta>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Filtrar_Ventas";
                    cmd.Parameters.AddWithValue("@Busqueda", string.IsNullOrEmpty(Busqueda) ? (object)DBNull.Value : Busqueda);
                    cmd.Parameters.AddWithValue("@NombreUsuario", string.IsNullOrEmpty(NombreUsuario) ? (object)DBNull.Value : NombreUsuario);
                    cmd.Parameters.AddWithValue("@Estado", Estado == null ? (object)DBNull.Value : Estado);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Usuario usuario = new Usuario()
                        {
                            NombreUsuario = reader["NombreUsuario"].ToString()
                        };

                        DateTime fechaVenta = Convert.ToDateTime(reader["FechaVenta"]);

                        listadoVentas.Add(new Venta
                        {
                            IdVenta = Convert.ToInt32(reader["IdVenta"]),
                            Cliente = reader["Cliente"].ToString(),
                            FechaVenta = fechaVenta,
                            Total = Convert.ToDecimal(reader["Total"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            usuario = usuario
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return listadoVentas;
        }
    }
}
