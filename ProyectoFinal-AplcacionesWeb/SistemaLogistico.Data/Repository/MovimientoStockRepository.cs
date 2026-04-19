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
    public class MovimientoStockRepository : IMovimientoStock
    {
        private readonly string cadenaConexion;

        public MovimientoStockRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:Database"] ?? string.Empty;
        }

        public List<MovimientoStock> ListadoMovimiento(string TipoMovimiento, string Busqueda)
        {
            List<MovimientoStock> listadoMovimientos = new List<MovimientoStock>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Filtrar_Movimientos";
                    cmd.Parameters.AddWithValue("@TipoMovimiento", string.IsNullOrEmpty(TipoMovimiento) ? (object)DBNull.Value : TipoMovimiento);
                    cmd.Parameters.AddWithValue("@Busqueda", string.IsNullOrEmpty(Busqueda) ? (object)DBNull.Value : Busqueda);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Producto producto = new Producto()
                        {
                            NombreProducto = reader["NombreProducto"].ToString()
                        };

                        Usuario usuario = new Usuario()
                        {
                            NombreUsuario = reader["NombreUsuario"].ToString()
                        };

                        DateTime fechaMovimiento = Convert.ToDateTime(reader["FechaMovimiento"]);

                        listadoMovimientos.Add(new MovimientoStock
                        {
                            IdMovimiento = Convert.ToInt32(reader["IdMovimiento"]),
                            producto = producto,
                            TipoDeMovimiento = reader["TipoDeMovimiento"].ToString(),
                            Cantidad = Convert.ToInt32(reader["Cantidad"]),
                            Motivo = reader["Motivo"].ToString(),
                            FechaMovimiento = fechaMovimiento,
                            usuario = usuario
                        });

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return listadoMovimientos;
        }

        public int RegistrarMovimiento(MovimientoStock movimiento)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_RegistrarMovimiento";
                    cmd.Parameters.AddWithValue("@TipoMovimiento", movimiento.TipoDeMovimiento);
                    cmd.Parameters.AddWithValue("@Cantidad", movimiento.Cantidad);
                    cmd.Parameters.AddWithValue("@Motivo", movimiento.Motivo);
                    cmd.Parameters.AddWithValue("@IdProducto", movimiento.IdProducto);
                    cmd.Parameters.AddWithValue("@IdUsuario", movimiento.IdUsuario);
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
