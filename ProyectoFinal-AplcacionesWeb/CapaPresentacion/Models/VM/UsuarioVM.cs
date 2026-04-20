using SistemaLogistico.Entities;

namespace CapaPresentacion.Models.VM
{
    public class UsuarioVM
    {
        public int IdUsuario { get; set; }

        public string NombreUsuario { get; set; }

        public int IdRol { get; set; }

        public string Documento { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public string Contraseña { get; set; }

        public DateTime FechaCreacion { get; set; }

        public bool Estado { get; set; }

        public RolVM rol { get; set; }

    }
}
