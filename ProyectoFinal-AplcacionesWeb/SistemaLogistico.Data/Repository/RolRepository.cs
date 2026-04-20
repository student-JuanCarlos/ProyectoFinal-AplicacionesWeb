using SistemaLogistico.Entities;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;
using SistemaLogistico.Data.Infraestructure;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;

namespace SistemaLogistico.Data.Repository
{
    public class RolRepository:IRol
    {
        private readonly string cadenaConexion;

        public RolRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:Database"] ?? string.Empty;
        }

        public List<Rol> Listado()
        {
            List<Rol> listado = new List<Rol>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = "SELECT * FROM Rol";
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        listado.Add(new Rol()
                        {
                            IdRol = Convert.ToInt32(reader[1]),
                            NombreRol = reader[2].ToString()
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
