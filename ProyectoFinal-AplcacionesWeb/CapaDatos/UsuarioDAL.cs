using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class UsuarioDAL
    {
        private readonly string cadenaConexion = "tuCadenaConexion";
        public int RegistroUsuario(Usuario usuario)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Registro_Usuario";
                    cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);
                    cmd.Parameters.AddWithValue("@Documento", usuario.Documento);
                    cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
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

        public int ActualizarUsuario(Usuario usuario)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Actualizar_Usuario";
                    cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                    cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);
                    cmd.Parameters.AddWithValue("@Documento", usuario.Documento);
                    cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
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

        public Usuario LoginUsuario(string Email, string Contraseña)
        {
            Usuario usuario = null;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Login_Usuario";
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Contraseña", Contraseña);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        usuario = new Usuario();

                        usuario.IdUsuario = Convert.ToInt32(reader["IdUsuario"].ToString());
                        usuario.NombreUsuario = reader["NombreUsuario"].ToString();
                        usuario.Documento = reader["Documento"].ToString();
                        usuario.Telefono = reader["Telefono"].ToString();
                        usuario.Estado = Convert.ToBoolean(reader["Estado"].ToString());

                        usuario.rol = new Rol()
                        {
                            NombreRol = reader["NombreRol"].ToString()
                        };
                    }
                    cn.Close();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return usuario;
        }

        public List<Usuario> ListadoUsuario(String Busqueda)
        {
            List<Usuario> listaUsuario = new List<Usuario>();
            using(SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Listado_Usuario";
                    cmd.Parameters.AddWithValue("@Busqueda",string.IsNullOrEmpty(Busqueda) ? (object)DBNull.Value : Busqueda);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Rol rol = new Rol()
                        {
                            NombreRol = reader["NombreRol"].ToString()
                        };

                        DateTime fechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);

                        listaUsuario.Add(new Usuario
                        { 
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"].ToString()),
                            NombreUsuario = reader["NombreUsuario"].ToString(),
                            rol = rol,
                            Email = reader["Email"].ToString(),
                            FechaCreacion = fechaCreacion,
                            Estado = Convert.ToBoolean(reader["Estado"])
                        }); 

                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return listaUsuario;
        }

        public Usuario DetalleUsuario(int id)
        {
            Usuario usuario = null;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_Detalle_Usuario";
                    cmd.Parameters.AddWithValue("@IdUsuario", id);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Rol rol = new Rol()
                        {
                            IdRol = Convert.ToInt32(reader["IdRol"]),
                            NombreRol = reader["NombreRol"].ToString()
                        };

                        DateTime fechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);

                        usuario = new Usuario()
                        {
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"].ToString()),
                            NombreUsuario = reader["NombreUsuario"].ToString(),
                            rol = rol,
                            Documento = reader["Documento"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Email = reader["Email"].ToString(),
                            FechaCreacion = fechaCreacion,
                            Estado = Convert.ToBoolean(reader["Estado"])
                        };
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return usuario;
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
                    cmd.CommandText = "sp_CambiarEstado_Usuario";
                    cmd.Parameters.AddWithValue("@IdUsuario", id);
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

        public int CambiarContraseña(int id, string contraseñaNueva)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_CambiarContraseña";
                    cmd.Parameters.AddWithValue("@IdUsuario", id);
                    cmd.Parameters.AddWithValue("@ContraseñaNueva", contraseñaNueva);
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
