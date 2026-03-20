using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace CapaDatos
{
    class ConexionBD
    {

        public static String cn = ConfigurationManager.ConnectionStrings["cadena"].ToString();

    }
}
