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
    public class VentaRepository : IVenta
    {
        private readonly string cadenaConexion;

        public VentaRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:Database"] ?? string.Empty;
        }

        public int RegistrarVenta(Venta venta, List<DetalleVenta> detalles, List<DetalleDescuento> descuentos)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    //TVP de DetalleVenta
                    DataTable dt = new DataTable();
                    dt.Columns.Add("IdProducto", typeof(int));
                    dt.Columns.Add("Cantidad", typeof(int));

                    foreach (var d in detalles)
                    {
                        dt.Rows.Add(d.IdProducto, d.Cantidad);
                    }

                    //TVP de DetalleDescuentos
                    DataTable dtdescuentos = new DataTable();
                    dtdescuentos.Columns.Add("IdDescuento", typeof(int));
                    dtdescuentos.Columns.Add("PorcentajeAplicado", typeof(decimal));

                    foreach (var des in descuentos)
                    {
                        dtdescuentos.Rows.Add(des.IdDescuento, des.PorcentajeAplicado);
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

                    SqlParameter tvpdes = cmd.Parameters.AddWithValue("@Descuento", dtdescuentos);
                    tvpdes.SqlDbType = SqlDbType.Structured;
                    tvpdes.TypeName = "TVP_DetalleDescuento";

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

        public Venta Detalle(int id)
        {
            Venta venta = null;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
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
                            Detalles = new List<DetalleVenta>(),
                            Descuentos = new List<DetalleDescuento>()
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

                        reader.NextResult();
                        while (reader.Read())
                        {
                            venta.Descuentos.Add(new DetalleDescuento
                            {
                                Descuento = new Descuento()
                                {
                                    NombreDescuento = reader["NombreDescuento"]?.ToString() ?? "",
                                    TipoDescuento = reader["TipoDescuento"]?.ToString() ?? ""
                                },
                                PorcentajeAplicado = reader["PorcentajeAplicado"] != DBNull.Value ? Convert.ToDecimal(reader["PorcentajeAplicado"]) :0,
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return venta;
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
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            NombreUsuario = reader["NombreUsuario"].ToString()
                        };

                        DateTime fechaVenta = Convert.ToDateTime(reader["FechaVenta"]);

                        listadoVentas.Add(new Venta
                        {
                            IdVenta = Convert.ToInt32(reader["IdVenta"]),
                            Cliente = reader["Cliente"].ToString(),
                            FechaVenta = fechaVenta,
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
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

        #region
        public List<Venta> Listado(string Busqueda)
        {
            throw new NotImplementedException();
        }


        public int CambiarEstado(int id)
        {
            throw new NotImplementedException();
        }

        public int Actualizar(Venta entity)
        {
            throw new NotImplementedException();
        }

        public int Agregar(Venta entity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
