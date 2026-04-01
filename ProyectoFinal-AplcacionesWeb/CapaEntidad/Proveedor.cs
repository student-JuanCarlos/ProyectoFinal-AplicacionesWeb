using System;
using System.Collections.Generic;
using System.Text;

namespace CapaEntidad
{
    public class Proveedor
    {

        public int IdProveedor { get; set; }

        public string NombreProveedor { get; set; }

        public string RUC { get; set; }

        public string Telefono { get; set; }

        public string PaginaWeb { get; set; }

        public string EmailEmpresa { get; set; }

        public string ProductoOfrecido { get; set; }

        public bool Estado { get; set; }

    }
}
